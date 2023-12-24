namespace ZStack.AspNetCore.SqlSugar;

public class SqlSugarInitializer(ILogger<SqlSugarInitializer> logger) : ISqlSugarInitializer
{
    public DbConnectionOptions Options { get; } = App.GetOptions<DbConnectionOptions>();
    public ILogger Logger { get; } = logger;

    public virtual void SetDbConfig(DbConnectionConfig config)
    {
        if (string.IsNullOrEmpty(config.ConfigId?.ToString()))
            config.ConfigId = SqlSugarConst.MainConfigId;
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityNameService = (type, entity) => DbConfig_EntityNameService(config, type, entity),
            EntityService = (type, column) => DbConfig_EntityService(config, type, column),
            DataInfoCacheService = new SqlSugarCache(),
        };
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true,
            IsAutoDeleteQueryFilter = true, // 启用删除查询过滤器
            IsAutoUpdateQueryFilter = true, // 启用更新查询过滤器
            SqlServerCodeFirstNvarchar = true // 采用Nvarchar
        };
    }

    public virtual void DbConfig_EntityNameService(DbConnectionConfig config, Type type, EntityInfo entity)
    {
        entity.IsDisabledDelete = true; // 禁止删除非 sqlsugar 创建的列
        // 只处理贴了特性[SugarTable]表
        if (!type.GetCustomAttributes<SugarTable>().Any())
            return;
        if (config.DbSettings.EnableUnderLine && !entity.DbTableName.Contains('_'))
            entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
    }

    public virtual void DbConfig_EntityService(DbConnectionConfig config, PropertyInfo type, EntityColumnInfo column)
    {
        // 只处理贴了特性[SugarColumn]列
        if (!type.GetCustomAttributes<SugarColumn>().Any())
            return;
        if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
            column.IsNullable = true;
        if (config.DbSettings.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
            column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线
    }

    public virtual void InitDatabase(DbConnectionConfig config, SqlSugarScope db)
    {
        // 初始化/创建数据库
        if (config.DbSettings.EnableInitDb)
        {
            if (config.DbType != DbType.Oracle)
                db.DbMaintenance.CreateDatabase();
        }

        // 初始化表结构
        if (config.TableSettings.EnableInitTable)
        {
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false))
                .WhereIF(config.TableSettings.EnableIncreTable, u => u.IsDefined(typeof(IncreTableAttribute), false)).ToList();

            if (config.ConfigId?.ToString() == SqlSugarConst.MainConfigId) // 默认库（有系统表特性、没有日志表和租户表特性）
                entityTypes = entityTypes.Where(u => u.GetCustomAttributes<SysTableAttribute>().Any() || !u.GetCustomAttributes<TenantAttribute>().Any()).ToList();
            else
                entityTypes = entityTypes.Where(u => u.GetCustomAttribute<TenantAttribute>()?.configId.ToString() == config.ConfigId?.ToString()).ToList(); // 自定义的库

            foreach (var entityType in entityTypes)
            {
                if (entityType.GetCustomAttribute<SplitTableAttribute>() == null)
                    db.CodeFirst.InitTables(entityType);
                else
                    db.CodeFirst.SplitTables().InitTables(entityType);
            }
        }

        // 初始化种子数据
        if (config.SeedSettings.EnableInitSeed)
        {
            var seedDataTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))))
                .WhereIF(config.SeedSettings.EnableIncreSeed, u => u.IsDefined(typeof(IncreSeedAttribute), false)).ToList();

            foreach (var seedType in seedDataTypes)
            {
                var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                if (config.ConfigId?.ToString() == SqlSugarConst.MainConfigId) // 默认库（有系统表特性、没有日志表和租户表特性）
                {
                    if (entityType.GetCustomAttribute<SysTableAttribute>() == null && entityType.GetCustomAttribute<TenantAttribute>() != null)
                        continue;
                }
                else
                {
                    var att = entityType.GetCustomAttribute<TenantAttribute>(); // 自定义的库
                    if (att == null || att.configId.ToString() != config.ConfigId?.ToString()) continue;
                }

                var instance = Activator.CreateInstance(seedType);
                var hasDataMethod = seedType.GetMethod("HasData");
                var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null)!)?.Cast<object>();
                if (seedData == null) continue;

                var entityInfo = db.EntityMaintenance.GetEntityInfo(entityType);
                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                {
                    // 按主键进行批量增加和更新
                    var storage = db.StorageableByObject(seedData.ToList()).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                    storage.AsUpdateable.ExecuteCommand();
                }
                else
                {
                    // 无主键则只进行插入
                    if (!db.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                        db.InsertableByObject(seedData.ToList()).ExecuteCommand();
                }
            }
        }
    }

    public virtual void SetDbAop(DbConnectionConfig config, SqlSugarClient db)
    {
        db.Ado.CommandTimeOut = Options.CommandTimeOut;
        db.Aop.OnLogExecuting = (sql, pars) => Aop_OnLogExecuting(config, db, sql, pars);
        db.Aop.OnError = (ex) => Aop_OnError(config, db, ex);
        db.Aop.OnLogExecuted = (sql, pars) => Aop_OnLogExecuted(config, db, sql, pars);
        db.Aop.DataExecuting = (oldValue, entityInfo) => Aop_DataExecuting(config, db, oldValue, entityInfo);
    }

    public virtual void Aop_OnLogExecuting(DbConnectionConfig config, SqlSugarClient db, string sql, SugarParameter[] parameters)
    {
        if (config.AopSettings.EnableSqlLog)
        {
            Logger.LogInformation("【执行SQL】{SQL}", UtilMethods.GetSqlString(config.DbType, sql, parameters));
            App.PrintToMiniProfiler("SqlSugar", "Info", sql + Environment.NewLine + db.Utilities.SerializeObject(parameters.ToDictionary(it => it.ParameterName, it => it.Value)));
        }
    }

    public virtual void Aop_OnError(DbConnectionConfig config, SqlSugarClient db, SqlSugarException ex)
    {
        if (config.AopSettings.EnableErrorSqlLog)
        {
            string sql = UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[]?)ex.Parametres ?? []);
            Logger.LogError("""
                    【错误SQL】
                    {SQL}
                    Message: {Message}
                    StackTrace: {StackTrace}
                    """, sql, ex.Message, ex.StackTrace);
            App.PrintToMiniProfiler("SqlSugar", "Error", $"{ex.Message}{Environment.NewLine}{ex.Sql}{Environment.NewLine}");
        }
    }

    public virtual void Aop_OnLogExecuted(DbConnectionConfig config, SqlSugarClient db, string sql, SugarParameter[] parameters)
    {
        if (config.AopSettings.EnableSlowSqlLog && db.Ado.SqlExecutionTime.TotalMilliseconds > config.AopSettings.SlowSqlTime)
        {
            Logger.LogWarning("【慢SQL】{SQL}", UtilMethods.GetSqlString(config.DbType, sql, parameters));
            App.PrintToMiniProfiler("SqlSugar", "Warn", sql + Environment.NewLine + db.Utilities.SerializeObject(parameters.ToDictionary(it => it.ParameterName, it => it.Value)));
        }
    }

    public virtual void Aop_DataExecuting(DbConnectionConfig config, SqlSugarClient db, object oldValue, DataFilterModel entityInfo)
    {
        var propertyType = entityInfo.EntityColumnInfo.PropertyInfo.PropertyType;
        if (entityInfo.OperationType == DataFilterType.InsertByObject)
        {
            if (entityInfo.EntityColumnInfo.IsPrimarykey && config.AopSettings.EnableIdAutoFill)
            {
                var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                if ((propertyType == typeof(long) || propertyType == typeof(long?)) && !entityInfo.EntityColumnInfo.IsIdentity)
                {
                    if (id == null || (long)id == 0)
                        entityInfo.SetValue(YitIdHelper.NextId());
                }
                else if (propertyType == typeof(Guid) || propertyType == typeof(long?))
                {
                    if (id == null || (Guid)id == Guid.Empty)
                        entityInfo.SetValue(Guid.NewGuid());
                }
                else if (propertyType == typeof(string))
                {
                    if (string.IsNullOrEmpty((string?)id))
                        entityInfo.SetValue(Guid.NewGuid().ToString("N"));
                }
            }
            else if (config.AopSettings.CreateTimeFields.Contains(entityInfo.PropertyName))
                entityInfo.SetValue(DateTime.Now);
            else if (config.AopSettings.UpdateTimeFields.Contains(entityInfo.PropertyName))
                entityInfo.SetValue(DateTime.Now);
        }
        else if (entityInfo.OperationType == DataFilterType.UpdateByObject)
        {
            if (config.AopSettings.UpdateTimeFields.Contains(entityInfo.PropertyName))
                entityInfo.SetValue(DateTime.Now);
        }
    }
}
