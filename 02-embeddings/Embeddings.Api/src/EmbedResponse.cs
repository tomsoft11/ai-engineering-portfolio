using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embeddings.Api.src
{
    public class EmbedResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("embeddings")]
        public List<float[]> Embeddings { get; set; }
    }
}
