using Furion.Schedule;
using Mapster;

namespace ZStack.AspNetCore.Schedule;

public class JobPersistence(IJobDetailRepo _repo) : IJobPersistence
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SchedulerBuilder> Preload()
    {
        // 获取所有定义的作业
        var allJobs = Furion.App.EffectiveTypes.ScanToBuilders().ToList();
        // 若数据库不存在任何作业，则直接返回
        if (_repo.GetJobs().Count == 0) return allJobs;

        // 遍历所有定义的作业
        foreach (var schedulerBuilder in allJobs)
        {
            // 获取作业信息构建器
            var jobBuilder = schedulerBuilder.GetJobBuilder();
            // 加载数据库数据
            var dbDetail = _repo.GetJob(jobBuilder.JobId);
            if (dbDetail == null) continue;

            // 同步数据库数据
            jobBuilder.LoadFrom(dbDetail);

            // 获取作业的所有数据库的触发器
            var dbTriggers = _repo.GetTriggers(jobBuilder.JobId);
            // 遍历所有作业触发器
            foreach (var (_, triggerBuilder) in schedulerBuilder.GetEnumerable())
            {
                var dbTrigger = dbTriggers.FirstOrDefault(u => u.TriggerId == triggerBuilder.TriggerId);
                if (dbTrigger == null) continue;

                triggerBuilder.LoadFrom(dbTrigger).Updated(); // 标记更新
            }
            // 遍历所有非编译时定义的触发器加入到作业中
            foreach (var dbTrigger in dbTriggers)
            {
                if (schedulerBuilder.GetTriggerBuilder(dbTrigger.TriggerId)?.JobId == jobBuilder.JobId) continue;
                var triggerBuilder = TriggerBuilder.Create(dbTrigger.TriggerId).LoadFrom(dbTrigger);
                schedulerBuilder.AddTriggerBuilder(triggerBuilder); // 先添加
                triggerBuilder.Updated(); // 再标记更新
            }

            // 标记更新
            schedulerBuilder.Updated();
        }

        // 获取数据库所有通过脚本创建的作业
        var allDbScriptJobs = _repo.GetJobs().Where(u => u.CreateType != JobCreateTypeEnum.BuiltIn).ToList();
        foreach (var dbDetail in allDbScriptJobs)
        {
            // 动态创建作业
            Type? jobType = null;
            jobType = dbDetail.CreateType switch
            {
                JobCreateTypeEnum.Script => BuildJob(dbDetail.ScriptCode ?? string.Empty),
                JobCreateTypeEnum.Http => typeof(HttpJob),
                _ => null,
            };

            if (jobType == null)
                continue;

            // 动态构建的 jobType 的程序集名称为随机名称，需重新设置
            dbDetail.AssemblyName = jobType.Assembly.FullName!.Split(',')[0];
            var jobBuilder = JobBuilder.Create(jobType).LoadFrom(dbDetail);

            // 强行设置为不扫描 IJob 实现类 [Trigger] 特性触发器，否则 SchedulerBuilder.Create 会再次扫描，导致重复添加同名触发器
            jobBuilder.SetIncludeAnnotations(false);

            // 获取作业的所有数据库的触发器加入到作业中
            var dbTriggers = _repo.GetTriggers(dbDetail.JobId);
            var triggerBuilders = dbTriggers.Select(u => TriggerBuilder.Create(u.TriggerId).LoadFrom(u).Updated());
            var schedulerBuilder = SchedulerBuilder.Create(jobBuilder, triggerBuilders.ToArray());

            // 标记更新
            schedulerBuilder.Updated();

            allJobs.Add(schedulerBuilder);
        }

        return allJobs;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public SchedulerBuilder OnLoading(SchedulerBuilder builder)
    {
        return builder;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="context"></param>
    public void OnChanged(PersistenceContext context)
    {
        var jobDetail = context.JobDetail.Adapt<JobDetail>();
        switch (context.Behavior)
        {
            case PersistenceBehavior.Appended:
                _repo.AddOrUpdateJob(jobDetail);
                break;

            case PersistenceBehavior.Updated:
                var dbJobDetail = _repo.GetJob(jobDetail.JobId);
                if (dbJobDetail != null)
                {
                    jobDetail.CreateType = dbJobDetail.CreateType;
                    jobDetail.ScriptCode = dbJobDetail.ScriptCode;
                    _repo.AddOrUpdateJob(jobDetail);
                }
                break;

            case PersistenceBehavior.Removed:
                _repo.RemoveJob(jobDetail.JobId);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnTriggerChanged(PersistenceTriggerContext context)
    {
        var jobTrigger = context.Trigger.Adapt<JobTrigger>();
        switch (context.Behavior)
        {
            case PersistenceBehavior.Appended:
            case PersistenceBehavior.Updated:
                _repo.AddOrUpdateTrigger(jobTrigger);
                break;
            case PersistenceBehavior.Removed:
                _repo.RemoveTrigger(jobTrigger.JobId, jobTrigger.TriggerId);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public static Type? BuildJob(string script)
    {
        var jobAssembly = Schedular.CompileCSharpClassCode(script);
        var jobType = jobAssembly.GetTypes().FirstOrDefault(u => typeof(IJob).IsAssignableFrom(u));
        return jobType;
    }
}
