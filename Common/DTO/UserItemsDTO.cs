using Newtonsoft.Json;
using System.Collections.Generic;

namespace NordPassHomeWorkTAF.Common.DTO
{
    [JsonObject]
    public class UserItemsDTO
    {
        [JsonProperty("items")]
        public List<string> Items { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
    }
}
