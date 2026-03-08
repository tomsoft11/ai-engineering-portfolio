
# Lab #3 – Embeddings
## Lokální generování embeddingů pomocí Ollamy

Tento lab navazuje na Lab #2 a připravuje data pro Lab #4 (Vector Search).  
Cílem je vytvořit API, které:

- generuje embeddingy pomocí modelu `nomic-embed-text`,
- ukládá je do JSONL,
- počítá kosinovou podobnost,
- poskytuje data pro další laby.

---

## Cíle labu

- Pochopit, jak embeddingy reprezentují text jako vektory.
- Naučit se generovat embeddingy lokálně bez cloudu.
- Ukládat embeddingy do JSONL.
- Implementovat kosinovou podobnost.
- Připravit datovou pipeline pro Qdrant.

---

## Architektura


[.NET API] /embed        → generuje embedding přes Ollamu /similarity   → počítá kosinovou podobnost data/         → ukládá embeddingy do JSONL
[Ollama] nomic-embed-text (embedding model)

---

## Spuštění projektu

### 1) Instalace embedding modelu

bash
ollama pull nomic-embed-text


### 2) Spuštění Ollamy
Ollama musí běžet na:
```bash
http://localhost:11434
```

### 3) Spuštění API
cd src
dotnet run


API poběží na:
```bash
http://localhost:5004
```


## Endpointy
### /embed
Vygeneruje embedding pro zadaný text a uloží ho do data/embeddings.jsonl.
Request
```bash
{
  "text": "Artificial intelligence is transforming the world."
}
```

Response
```bash
{
  "id": "uuid",
  "vector": [0.0123, -0.9981, ...],
  "text": "Artificial intelligence is transforming the world."
}
```


### /similarity

Vrátí kosinovou podobnost dvou textových řetězců.

Request
```bash
{
  "a": "Artificial intelligence is transforming the world.",
  "b": "Artificial intelligence is transforming the world!"
}
```

Response
```bash
{
  "score": 0.9999
}
```




## Ukázkové requesty (REST Client)
examples/sample-requests.http:
### Generate embedding
POST http://localhost:5004/embed

Content-Type: application/json
```bash
{
  "text": "The cat sits on the mat."
}
```
### Similarity
POST http://localhost:5004/similarity

Content-Type: application/json
```bash
{
  "a": "Artificial intelligence is transforming the world.",
  "b": "Artificial intelligence is transforming the world!"
}
```

