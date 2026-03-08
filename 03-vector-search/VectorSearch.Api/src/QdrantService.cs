using Google.Protobuf.WellKnownTypes;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using VectorSearch.Api.src;

public class QdrantService
{
    private readonly QdrantClient _client;

    public QdrantService()
    {
        _client = new QdrantClient(new Uri("http://localhost:6334"));
    }

    public async Task UpsertAsync(string id, float[] vector, string text)
    {
        var point = new PointStruct
        {
            Id = new PointId { Uuid = id },
            Vectors = new Vectors
            {
                Vector = new Vector(vector)
            }
        };

        point.Payload.Add("text", text);

        await _client.UpsertAsync(
            collectionName: "embeddings",
            points: new[] { point }
        );
    }

    public async Task<List<SearchResultDto>> SearchAsync(float[] vector, ulong limit = 5)
    {
        var results = await _client.SearchAsync(
            collectionName: "embeddings",
            vector: vector,
            limit: limit
        );

        var output = new List<SearchResultDto>();

        foreach (var r in results)
        {
            if (r.Payload.TryGetValue("text", out var textValue))
            {
                output.Add(new SearchResultDto() { Text = textValue.StringValue, Score = r.Score });
            }
        }

        return output;
    }
}