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

int EstimateTokens(string text)
{
    return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
}

app.MapPost("/embed", async (EmbedRequest req, HttpClient http) =>
{
    var payload = new
    {
        model = req.Model ?? "nomic-embed-text",
        input = req.Text
    };

    var response = await http.PostAsJsonAsync("http://localhost:11434/api/embed", payload);
    var content = await response.Content.ReadAsStringAsync();
    var data = JsonSerializer.Deserialize<EmbedResponse>(content);

    var record = new EmbeddingRecord
    {
        Id = Guid.NewGuid().ToString(),
        Text = req.Text,
        Vector = data.Embeddings.First()
    };

    var json = JsonSerializer.Serialize(record);
    await File.AppendAllTextAsync("data/embeddings.jsonl", json + "\n");

    return Results.Ok(new
    {
        embedding = content
    });
});

app.Run();
