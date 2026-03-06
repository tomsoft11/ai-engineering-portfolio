namespace Embeddings.Api.src
{
    public class EmbeddingRecord
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public float[] Vector { get; set; }
    }
}
