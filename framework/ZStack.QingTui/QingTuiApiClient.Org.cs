namespace ZStack.QingTui;

public partial class QingTuiApiClient
{
    /// <summary>
    /// 获取企业Id
    /// </summary>
    /// <param name="number">企业号，可在管理后台中企业管理模块中查看</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetDomainIdAsync(string number, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/domain/id/get")
            .SetQueryParam("number", number)
            .GetJsonAsync<DomainIdResp>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.DomainId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 创建组织机构
    /// </summary>
    /// <param name="parentId">父机构Id，顶级为root</param>
    /// <param name="name">组织机构名称</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> CreateOrgAsync(string parentId, string name, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/org/create")
            .PostUrlEncodedAsync(new
            {
                parent_id = parentId,
                name
            }, cancellationToken: cancellationToken)
            .ReceiveJson<CreateOrgResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.OrgId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 修改组织机构
    /// </summary>
    /// <param name="org"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task UpdateOrgAsync(UpdateOrgReq org, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/org/update")
            .PostUrlEncodedAsync(new
            {
                org_id = org.OrgId,
                name = org.Name,
                sequence = org.Sequence,
                autoSequence = org.AutoSequence
            }, cancellationToken: cancellationToken)
            .ReceiveJson<ErrorResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
    }

    /// <summary>
    /// 删除组织机构
    /// </summary>
    /// <param name="orgId">组织机构Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteOrgAsync(string orgId, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/org/delete")
            .PostUrlEncodedAsync(new
            {
                org_id = orgId
            }, cancellationToken: cancellationToken)
            .ReceiveJson<ErrorResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
    }

    /// <summary>
    /// 分页获取组织机构列表
    /// </summary>
    /// <param name="orgId">组织机构id，顶级为root。获取此id的子组织机构信息；如果不填，则拉取整个组织机构</param>
    /// <param name="page">请求的页码，从1开始</param>
    /// <param name="pageSize">请求的页码数据量，最大100</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PagedResp<QtOrgInfo>> GetPagedOrgsAsync(string? orgId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/org/paged")
            .SetQueryParam("org_id", orgId)
            .SetQueryParam("request_page", page)
            .SetQueryParam("page_size", pageSize)
            .GetJsonAsync<PagedResp<QtOrgInfo>>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 获取组织机构详情
    /// </summary>
    /// <param name="orgId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<QtOrgInfo> GetOrgAsync(string orgId, CancellationToken cancellationToken = default)
    {
        var json = await RestClient.Request("/team/org/detail")
            .SetQueryParam("org_id", orgId)
            .GetStringAsync(cancellationToken: cancellationToken);
        var res = JsonSerializer.Deserialize<ErrorResp>(json) ?? new();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        var org = JsonSerializer.Deserialize<QtOrgInfo>(json)
            ?? throw Oops.Bah("无响应");
        return org;
    }

    /// <summary>
    /// 组织机构变更同步
    /// </summary>
    /// <param name="syncTime">同步时间，从此时间（含）后的变更都会返回</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<QtOrgInfo>> GetOrgsBySyncTimeAsync(DateTime syncTime, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/org/sync")
            .SetQueryParam("sync_time", syncTime.ToTimestampMilliseconds())
            .GetJsonAsync<SyncOrgResp>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.Items ?? [];
    }
}
