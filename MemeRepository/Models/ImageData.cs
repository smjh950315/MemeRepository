using System.Text.Json.Serialization;

namespace MemeRepository.Models
{
    public class ImageData
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
    }
}
