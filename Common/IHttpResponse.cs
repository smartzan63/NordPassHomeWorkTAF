using System.Net;

namespace NordPassHomeWorkTAF.Common
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string? Content { get; set; }
    }
}
