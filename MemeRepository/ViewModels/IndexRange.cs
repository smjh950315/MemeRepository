using System.Text.Json.Serialization;

namespace MemeRepository.ViewModels
{
    public struct IndexRange
    {
        [JsonPropertyName("begin")]
        public int Begin { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
