namespace LocalLLM.Api.src
{
    public class GenerateRequest
    {
        public string Prompt { get; set; }
        public string? Model { get; set; }
    }
}
