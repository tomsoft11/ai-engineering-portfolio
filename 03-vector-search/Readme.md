# Vector Search (Qdrant)
## Lokální vektorové vyhledávání pomocí Qdrantu a gRPC klienta

Tento lab navazuje na Lab #3 (Embeddingy) a přidává schopnost ukládat embeddingy do vektorové databáze Qdrant a provádět nad nimi vektorové vyhledávání.  
Cílem je vytvořit API, které:

- ukládá embeddingy do Qdrantu,
- vyhledává podobné vektory,
- vrací výsledky s payloadem a skóre,
- připravuje základ pro RAG v dalších labech.

---

## Cíle labu

- Naučit se pracovat s Qdrantem přes gRPC API.
- Vytvořit kolekci pro embeddingy.
- Implementovat endpoint pro nahrávání embeddingů.
- Implementovat endpoint pro vektorové vyhledávání.
- Pochopit rozdíl mezi HTTP API a gRPC API v Qdrantu.
- Připravit datovou vrstvu pro RAG.

---

---

## Spuštění Qdrantu

### 1) Docker Compose

```bash
docker compose up -d
```

Qdrant poběží na:
- gRPC: http://localhost:6334
- REST: http://localhost:6333

### Vytvoření kolekce
Kolekce musí být vytvořena před prvním upsertem.
Příklad pro vektory o velikosti 768:
```bash
curl -X PUT "http://localhost:6334/collections/embeddings" \
  -H "Content-Type: application/json" \
  -d '{
        "vectors": {
          "size": 768,
          "distance": "Cosine"
        }
      }'
```


## Endpointy
### /upload
Uloží embedding do Qdrantu.
Request
```bash
{
  "text": "Cats are small animals.",
  "vector": [0.123, -0.456, ...]
}
```

Response
```bash
{
  "id": "generated-uuid"
}
```


### /search
Vyhledá podobné embeddingy.

Request
```bash
{
  "query": "Tell me about animals."
}
```

Response
```bash
[
  {
    "text": "Cats are small animals.",
    "score": 0.81234
  },
  {
    "text": "Dogs are domestic animals.",
    "score": 0.73455
  }
]
```



## Klíčové části implementace
### QdrantService – gRPC klient
- používá port 6334 (gRPC)
- používá PointStruct, Vectors, Payload
- payload se plní přes point.Payload.Add(...)
### /upload
- přijme text + embedding
- vygeneruje UUID
- uloží bod do kolekce embeddings
### /search
- zavolá Ollamu pro embedding dotazu
- pošle vektor do Qdrantu
- vrátí výsledky jako DTO

## Ukázkové requesty (REST Client)
### Upload
POST http://localhost:5004/upload

Content-Type: application/json
```bash
{
  "text": "Cats are small animals.",
  "vector": [...]
}
```
### Search
POST http://localhost:5004/search

Content-Type: application/json
```bash
{
  "query": "Tell me about animals."
}
```
