# ZStack.SqlSugar

基于 SqlSugar ORM 的数据库基础库，提供多数据库支持、雪花ID生成、实体仓储、分页等功能。

## 快速开始

配置：

```json
{
  "DbConnection": {
    "ConnectionConfigs": [
      {
        "ConfigId": "main",
        "DbType": "MySql",
        "ConnectionString": "Server=localhost;Database=mydb;Uid=root;Pwd=root;",
        "IsAutoCloseConnection": true
      }
    ]
  }
}
```

## ISqlSugarService

```csharp
// 获取默认数据库上下文
var db = sqlSugarService.Get();

// 获取指定 configId 的数据库上下文
var db = sqlSugarService.Get("main");

// 尝试获取数据库上下文
bool exists = sqlSugarService.TryGet("main", out var scope);

// 检查是否存在指定配置
bool exists = sqlSugarService.Contains("main");
```

## DbRepository 泛型仓储

```csharp
public class UserRepository : DbRepository<User>
{
    // 继承即获得 CRUD 基础能力
    // 通过 base.Context 访问 SqlSugarClient
}
```

使用 `[Tenant(configId)]` 特性指定实体使用哪个数据库连接，`[SysTable]` 特性标记系统表使用默认库。

## 雪花ID

框架自动集成 YitIdHelper 雪花ID生成：

```json
{
  "SnowId": {
    "WorkerId": 1,
    "WorkerIdBitLength": 6,
    "SeqBitLength": 6
  }
}
```

`ZStack.AspNetCore.SqlSugar` 包中的 `IdGeneratorWorker` 提供基于 Redis 锁的分布式 WorkerId 分配。

## SqlSugarPagedList 分页

```csharp
var pagedList = await queryable.ToPagedListAsync(pageIndex: 1, pageSize: 20);

// 支持 Select 投影分页
var result = await queryable.ToPagedListAsync(1, 20, u => new UserDto { Name = u.Name });

// 内存分页
var paged = list.ToPagedList(1, 20);

// 类型转换
var casted = pagedList.Cast<OtherType>();

// 属性
pagedList.Page;         // 当前页码
pagedList.PageSize;     // 页容量
pagedList.Total;        // 总条数
pagedList.TotalPages;   // 总页数
pagedList.Items;        // 当前页数据
pagedList.HasPrevPage;  // 是否有上一页
pagedList.HasNextPage;  // 是否有下一页
```

## 自定义初始化器

实现 `ISqlSugarInitializer` 接口自定义数据库初始化和 AOP 配置：

```csharp
public class CustomInitializer : ISqlSugarInitializer
{
    public void SetDbConfig(DbConnectionConfig config) { }
    public void InitDatabase(DbConnectionConfig config, SqlSugarScope db) { }
    public void SetDbAop(DbConnectionConfig config, SqlSugarClient db) { }
}
```

注册：

```csharp
services.AddZStackSqlSugar<CustomInitializer>(configuration);
```

## 种子数据

实现 `ISqlSugarEntitySeedData<T>` 接口：

```csharp
public class UserSeed : ISqlSugarEntitySeedData<User>
{
    public IEnumerable<User> GetSeedData()
    {
        return [new User { Name = "admin" }];
    }
}
```
