using System.Net;
using System.Net.Http.Headers;

namespace NordPassHomeWorkTAF.Common
{
    public class HttpResponse : IHttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Content { get; set; }
        public HttpHeaders? Headers { get; set; }
    }
}
