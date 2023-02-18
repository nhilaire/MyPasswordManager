using Newtonsoft.Json;

namespace MyPasswordManager.Infrastructure.CosmosDb
{
    public class SecretDAO
    {
        public const string UniquePartition = "UniquePartition";

        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }
        [JsonProperty(PropertyName = "pkey")]
        public string? Partition { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Url { get; set; }
        public string? Notes { get; set; }
    }
}
