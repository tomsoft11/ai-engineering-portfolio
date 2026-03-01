# Local LLM Inference API

Tento projekt demonstruje lokální běh velkých jazykových modelů pomocí Ollama a jejich integraci do .NET API. Cílem je ukázat, jak efektivně volat modely, měřit výkon, optimalizovat latenci a připravit základ pro další AI systémy (RAG, agenti, orchestrace).
Projekt je navržený jako první krok v AI engineering roadmapě a tvoří základní stavební kámen pro všechny následující projekty.

## Funkce
- Lokální inference LLM přes Ollama (např. Llama 3.1 8B, Phi‑3 Mini 7B)
- .NET API endpoint pro generování odpovědí
- Měření latence, tokenů/s a celkové doby zpracování
- Podpora různých kvantizací modelů
- Oddělená vrstva pro komunikaci s Ollamou (snadná rozšiřitelnost)
- Připraveno pro integraci do RAG nebo agentních systémů

## Architektura
### Komponenty
- Ollama – lokální inference engine
- .NET 8 API – orchestrace requestů a měření výkonu
- LLM Service – vrstva pro komunikaci s Ollamou
- Benchmarking – jednoduchý modul pro měření tokenů/s a latence
### Diagram
[Client] → [LocalLLM.Api] → [LLM Service] → [Ollama] → [Model]

## Požadavky
- Windows 11 / Linux / macOS
- .NET 8 SDK
- Ollama nainstalovaná lokálně
- NVIDIA GPU (volitelné, ale doporučené pro rychlost)

## Instalace
1) Instalace Ollamy
Stáhni z https://ollama.com/download a nainstaluj.
Ověření:
ollama --version


2) Stažení modelu
Například:
ollama pull llama3.1


nebo:
ollama pull phi3


3) Spuštění API
V kořenové složce projektu:
dotnet run


API poběží na:
http://localhost:5000



## Použití
Endpoint: /generate
POST
http://localhost:5000/generate
Body:
{
  "prompt": "Explain how transformers work."
}


Ukázková odpověď:
{
  "response": "...",
  "tokensPerSecond": 42.1,
  "latencyMs": 812
}



Ukázka kódu (zkrácená)
var request = new
{
    model = "llama3.1",
    prompt = prompt
};

var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", request);
var content = await response.Content.ReadAsStringAsync();



## Benchmarky
V adresáři benchmarks/ najdeš:
- porovnání Llama 3.1 8B vs. Phi‑3 Mini 7B
- měření tokenů/s na CPU i GPU
- vliv kvantizace na rychlost

## Co tento projekt ukazuje
- schopnost integrovat LLM do backendu
- práci s lokální inference bez cloudu
- měření výkonu a optimalizaci
- čistou architekturu vhodnou pro rozšíření
Tento projekt je základním stavebním kamenem pro RAG backend, agenty a orchestrátor, které následují v dalších částech portfolia.
