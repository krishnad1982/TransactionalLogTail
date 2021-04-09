using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Poc.LogTail.Core.Models
{
    public class Tokens
    {
        [JsonProperty("_data")]
        public string Data { get; set; }
    }
}
