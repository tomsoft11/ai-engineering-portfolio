using LocalLLM.Api.src;
using System.Diagnostics;

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

app.MapPost("/generate", async (GenerateRequest req, HttpClient http) =>
{
    var sw = Stopwatch.StartNew();

    var payload = new
    {
        model = req.Model ?? "llama3.1",
        prompt = req.Prompt,
        stream = false
    };

    var response = await http.PostAsJsonAsync("http://localhost:11434/api/generate", payload);
    var content = await response.Content.ReadAsStringAsync();

    sw.Stop();

    var tokens = EstimateTokens(content);
    var tokensPerSecond = tokens / (sw.ElapsedMilliseconds / 1000.0);

    return Results.Ok(new
    {
        response = content,
        latencyMs = sw.ElapsedMilliseconds,
        tokens,
        tokensPerSecond

    });
});

app.Run();
