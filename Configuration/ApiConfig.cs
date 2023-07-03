namespace NordPassHomeWorkTAF.Configuration
{
    public class ApiConfig
    {
        public Dictionary<string, string> HostNames { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> BaseUrls { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, Uri> Endpoints { get; set; } = new Dictionary<string, Uri>();
    }
}