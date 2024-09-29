namespace ZStack.QingTui;

public partial class QingTuiApiClient
{
    /// <summary>
    /// 根据qt_code获取用户基本信息
    /// </summary>
    /// <param name="qtCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<QtLoginUserInfo> GetLoginUserInfoAsync(string qtCode, CancellationToken cancellationToken = default)
    {
        var json = await RestClient.Request("/v1/oauth2/userinfo")
            .SetQueryParam("appid", AppId)
            .SetQueryParam("grant_type", "client_credential")
            .SetQueryParam("qt_code", qtCode)
            .SetQueryParam("secret", Options.Secret)
            .GetStringAsync(cancellationToken: cancellationToken);
        var res = JsonSerializer.Deserialize<ErrorResp>(json)
            ?? throw Oops.Bah("无响应");
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        var userInfo = JsonSerializer.Deserialize<QtLoginUserInfo>(json)
            ?? throw Oops.Bah("无响应");
        return userInfo;
    }

    /// <summary>
    /// 分页获取使用者列表
    /// </summary>
    /// <param name="page">请求的页数，从1开始</param>
    /// <param name="pageSize">分页返回时每页数据量，最大100</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FollowersResp> GetPagedFollowersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/v1/app/followers")
            .SetQueryParam("request_page", page)
            .SetQueryParam("page_size", pageSize)
            .GetJsonAsync<FollowersResp>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 通过userId获取openId
    /// </summary>
    /// <param name="userId">企业内用户Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetOpenIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/openid/get")
            .SetQueryParam("user_id", userId)
            .GetJsonAsync<OpenIdResp>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.OpenId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 分页获取企业内所有成员
    /// </summary>
    /// <param name="page">请求的页数，从1开始</param>
    /// <param name="pageSize">分页返回时每页数据量，最大100</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PagedResp<QtMemberInfo>> GetPagedMembersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/all/paged")
            .SetQueryParam("request_page", page)
            .SetQueryParam("page_size", pageSize)
            .GetJsonAsync<PagedResp<QtMemberInfo>>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 成员信息变更同步
    /// </summary>
    /// <param name="syncTime">要同步的时间，从此时间（含）后的变更都会返回</param>
    /// <param name="page">请求的页码，从1开始</param>
    /// <param name="pageSize">请求的每页成员数量</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PagedResp<QtMemberInfo>> GetPagedMembers(DateTime syncTime, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/sync")
            .SetQueryParam("sync_time", syncTime.ToTimestampMilliseconds())
            .SetQueryParam("request_page", page)
            .SetQueryParam("page_size", pageSize)
            .GetJsonAsync<PagedResp<QtMemberInfo>>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 分页获取组织机构内成员
    /// </summary>
    /// <param name="orgId">组织机构Id，值为root时，为第一级组织机构</param>
    /// <param name="page">请求的页数，从1开始</param>
    /// <param name="pageSize">分页返回时每页数据量，最大100</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PagedResp<QtMemberInfo>> GetPagedOrgMembersAsync(string orgId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/org/paged")
            .SetQueryParam("org_id", orgId)
            .SetQueryParam("request_page", page)
            .SetQueryParam("page_size", pageSize)
            .GetJsonAsync<PagedResp<QtMemberInfo>>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 创建成员
    /// </summary>
    /// <param name="member"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> CreateMemberAsync(CreateMemberReq member, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/create")
            .PostUrlEncodedAsync(new
            {
                name = member.Name,
                mobile = member.Mobile,
                password = member.Password,
                org_id = member.OrgId,
                mail = member.Mail,
                comment = member.Comment,
                employee_id = member.EmployeeId
            }, cancellationToken: cancellationToken)
            .ReceiveJson<CreateMemberResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.UserId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 更新成员
    /// </summary>
    /// <param name="member"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task UpdaeMemberAsync(UpdateMemberReq member, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/update/single")
            .PostUrlEncodedAsync(new
            {
                user_id = member.UserId,
                name = member.Name,
                org_id = member.OrgId,
                comment = member.Comment,
                employee_id = member.EmployeeId
            }, cancellationToken: cancellationToken)
            .ReceiveJson<ErrorResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
    }

    /// <summary>
    /// 删除成员
    /// </summary>
    /// <param name="userId">企业内用户Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteMemberAsync(string userId, CancellationToken cancellationToken = default)
    {
        var res = await RestClient.Request("/team/member/delete")
            .PostUrlEncodedAsync(new
            {
                user_id = userId
            }, cancellationToken: cancellationToken)
            .ReceiveJson<ErrorResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    /// <param name="userId">企业内用户Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<QtMemberInfo> GetMemberDetailAsync(string userId, CancellationToken cancellationToken = default)
    {
        var json = await RestClient.Request("/team/member/detail")
            .SetQueryParam("user_id", userId)
            .GetStringAsync(cancellationToken: cancellationToken);
        var res = JsonSerializer.Deserialize<ErrorResp>(json) ?? new();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        var member = JsonSerializer.Deserialize<QtMemberInfo>(json)
            ?? throw Oops.Bah("无响应");
        return member;
    }

    /// <summary>
    /// 根据OpenId获取用户详情
    /// </summary>
    /// <param name="openId">使用者的openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<QtMemberInfo> GetMemberDetailByOpenIdAsync(string openId, CancellationToken cancellationToken = default)
    {
        var json = await RestClient.Request("/team/member/openid/detail")
            .SetQueryParam("open_id", openId)
            .GetStringAsync(cancellationToken: cancellationToken);
        var res = JsonSerializer.Deserialize<ErrorResp>(json) ?? new();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        var member = JsonSerializer.Deserialize<QtMemberInfo>(json)
            ?? throw Oops.Bah("无响应");
        return member;
    }
}
