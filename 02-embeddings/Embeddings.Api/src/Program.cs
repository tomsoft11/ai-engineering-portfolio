using Embeddings.Api.src;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();

builder.WebHost.UseUrls("http://localhost:5004");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection(); // pouzivame http

float CosineSimilarity(float[] a, float[] b)
{
    float dot = 0;
    float magA = 0;
    float magB = 0;

    for (int i = 0; i < a.Length; i++)
    {
        dot += a[i] * b[i];
        magA += a[i] * a[i];
        magB += b[i] * b[i];
    }

    return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
}

int EstimateTokens(string text)
{
    return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
}

async Task<EmbeddingRecord> GetOrCreateEmbeddingAsync(string text, HttpClient http)
{
    var dir = "data";
    var path = Path.Combine(dir, "embeddings.jsonl");
    Directory.CreateDirectory(dir);

    if (File.Exists(path))
    {
        foreach (var line in await File.ReadAllLinesAsync(path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                var rec = JsonSerializer.Deserialize<EmbeddingRecord>(line);
                if (rec != null && rec.Text == text && rec.Vector != null && rec.Vector.Length > 0)
                {
                    return rec;
                }
            }
            catch
            {
                // ignore malformed lines
            }
        }
    }

    // Not found -> compute using external embed service (do not call local /embed)
    var payload = new
    {
        model = "nomic-embed-text",
        input = text
    };

    var response = await http.PostAsJsonAsync("http://localhost:11434/api/embed", payload);
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var data = JsonSerializer.Deserialize<EmbedResponse>(content);

    var vector = data.Embeddings.First();

    var record = new EmbeddingRecord
    {
        Id = Guid.NewGuid().ToString(),
        Text = text,
        Vector = vector
    };

    var json = JsonSerializer.Serialize(record);
    await File.AppendAllTextAsync(path, json + "\n");

    return record;
}

// Keep original /embed if still useful elsewhere
app.MapPost("/embed", async (EmbedRequest req, HttpClient http) =>
{
    var result = await GetOrCreateEmbeddingAsync(req.Text, http);

    return Results.Ok(new
    {
        embedding = result
    });
});

// New similarity endpoint: accepts two texts, uses local cache or computes+stores, returns score
app.MapPost("/similarity", async (SimilarityRequest req, HttpClient http) =>
{
    if (string.IsNullOrEmpty(req.TextA) || string.IsNullOrEmpty(req.TextB))
    {
        return Results.BadRequest(new { error = "Both TextA and TextB are required." });
    }

    var a = await GetOrCreateEmbeddingAsync(req.TextA, http);
    var b = await GetOrCreateEmbeddingAsync(req.TextB, http);

    if (a.Vector.Length != b.Vector.Length)
    {
        return Results.Problem("Embedding vectors have different dimensions.");
    }

    var score = CosineSimilarity(a.Vector, b.Vector);
    return Results.Ok(new { similarity = score });
});

app.Run();

public class SimilarityRequest
{
    public string TextA { get; set; } = "";
    public string TextB { get; set; } = "";
}
