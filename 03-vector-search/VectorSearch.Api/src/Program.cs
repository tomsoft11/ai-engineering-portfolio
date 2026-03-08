using System.Diagnostics;
using System.Text.Json;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();

// register QdrantService so it can be injected into endpoints
builder.Services.AddSingleton<QdrantService>();

builder.WebHost.UseUrls("http://localhost:5004");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection(); // pouzivame http

app.MapPost("/upload", async (UploadRequest req, QdrantService qdrant) =>
{
    var id = Guid.NewGuid().ToString();
    await qdrant.UpsertAsync(id, req.Vector, req.Text);

    return Results.Ok(new { id });
});

app.MapPost("/search", async (SearchRequest req, HttpClient http, QdrantService qdrant) =>
{
    var embedPayload = new
    {
        model = "nomic-embed-text",
        input = req.Query
    };

    var embedResponse = await http.PostAsJsonAsync("http://localhost:11434/api/embed", embedPayload);
    var embedJson = await embedResponse.Content.ReadAsStringAsync();

    var parsed = JsonDocument.Parse(embedJson);
    var vector = parsed.RootElement
        .GetProperty("embeddings")[0]
        .EnumerateArray()
        .Select(x => x.GetSingle())
        .ToArray();

    var results = await qdrant.SearchAsync(vector);

    return Results.Ok(results);
});




app.Run();


public class SearchRequest
{
    public string Query { get; set; }
}
public class UploadRequest
{
    public string Text { get; set; }
    public float[] Vector { get; set; }
}
