using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NordPassHomeWorkTAF.Common.DTO
{
    public class ItemDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("files")]
        public List<FileDTO> Files { get; set; }

        [JsonProperty("fields")]
        public List<FieldDTO> Fields { get; set; }

        [JsonProperty("shares")]
        public List<ShareDTO> Shares { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class FileDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("content_path")]
        public string ContentPath { get; set; }
    }

    public class FieldDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class ShareDTO
    {
        [JsonProperty("user_uuid")]
        public string UserUuid { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
