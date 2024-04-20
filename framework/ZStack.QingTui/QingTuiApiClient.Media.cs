namespace ZStack.QingTui;

public partial class QingTuiApiClient
{
    /// <summary>
    /// 上传媒体文件
    /// </summary>
    /// <param name="type">媒体文件类型</param>
    /// <param name="stream">文件流</param>
    /// <param name="fileName">文件名</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> UploadMediaAsync(UploadMediaType type, Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        if (type == UploadMediaType.Image && stream.Length > 300 * 1024)
            throw Oops.Bah("图片大小不能超过300KB");
        if (type == UploadMediaType.File && stream.Length > 30 * 1024 * 1024)
            throw Oops.Bah("文件大小不能超过30MB");
        var res = await RestClient.Request("/v1/media/upload")
            .SetQueryParam("type", type.ToString().ToLower())
            .PostMultipartAsync(mp =>
            {
                mp.AddFile("media", stream, fileName, MimeTypesMap.GetMimeType(fileName));
            }, cancellationToken: cancellationToken)
            .ReceiveJson<MediaUploadResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MediaId ?? throw Oops.Bah("无响应");
    }
}
