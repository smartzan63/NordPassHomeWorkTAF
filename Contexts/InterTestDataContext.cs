using NordPassHomeWorkTAF.Common;

namespace NordPassHomeWorkTAF.Contexts
{
    public class InterTestDataContext
    {
        public HttpResponse? LastResponse { get; set; }
        public string? JwtToken { get; set; }
        public int? RateLimit { get; set; }
        public string? SignatureKey { get; set; }
        public string? Nonce { get; set; }
        public string? HmacSignature { get; set; }

    }

}
