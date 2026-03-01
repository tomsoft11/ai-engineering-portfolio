# AI Engineering Portfolio

Tento repozitář obsahuje ucelenou sadu projektů, které demonstrují praktické dovednosti v oblasti vývoje systémů založených na velkých jazykových modelech. Projekty pokrývají klíčové oblasti moderního AI engineeringu: lokální inference, vektorové databáze, RAG, agenti, orchestrátory, fine‑tuning a produkční deployment. Vše je postavené na technologiích, které se běžně používají v praxi: .NET, Ollama, Qdrant, MLflow, Docker a moderní open‑source modely.

## Projekty

Každý projekt je samostatný, má vlastní dokumentaci a lze jej spustit nezávisle. Projekty jsou řazeny tak, aby odrážely postupný růst dovedností.

1. Local LLM Inference API
Jednoduché .NET API, které volá lokální LLM přes Ollamu. Obsahuje měření latence, tokenů/s a ukázky optimalizace výkonu.
2. Vector Search Service
Služba pro generování embeddingů a vyhledávání pomocí vektorové databáze Qdrant. Obsahuje implementaci chunkingu a základní retrieval.
3. RAG Backend
Kompletní Retrieval‑Augmented Generation backend v .NET. Kombinuje embeddingy, vektorové vyhledávání a LLM inference. Obsahuje re‑ranking a ladění kvality odpovědí.
4. AI Agent with Tools
Agentní systém s podporou nástrojů (REST API, výpočty, práce s databází). Implementuje agent loop a plánování kroků.
5. Custom AI Orchestrator
Vlastní orchestrátor postavený v .NET pomocí kanálů a background services. Umožňuje řídit více kroků, nástrojů a modelů v rámci jednoho workflow.
6. LoRA Fine‑tuning
Ukázka fine‑tuning pipeline pomocí LoRA. Obsahuje přípravu datasetu, trénink, evaluaci a verzování modelů v MLflow.
7. Production‑ready AI Stack
Kompletní produkční stack v Dockeru: Ollama, Qdrant a .NET API. Obsahuje observabilitu, health‑checky a základní monitoring.

## Použité technologie

- .NET 8 – backend, orchestrace, API
- Ollama – lokální inference LLM
- Qdrant – vektorová databáze pro RAG
- MLflow – model registry a experiment tracking
- Docker / Docker Compose – deployment a orchestrace
- Hugging Face – modely, embeddingy, LoRA
- LangChain / LangGraph – agenti a workflow (tam, kde je to vhodné)

## Architektura celého stacku

### Lokální vývoj

- Ollama běží jako inference engine
- Qdrant poskytuje vektorové vyhledávání
- .NET API orchestruje dotazy, retrieval a generování odpovědí
  
### Produkční stack

- Docker Compose řídí všechny služby
- API poskytuje jednotný vstupní bod
- Observabilita sleduje latenci, tokeny a využití modelů
- MLflow spravuje verze modelů a experimenty

## Cíle portfolia

- ukázat schopnost stavět kompletní AI systémy od nuly
- demonstrovat práci s moderními LLM technologiemi
- prezentovat čistý, udržitelný a výkonný backendový kód
- pokrýt všechny oblasti, které firmy očekávají od AI vývojáře
- poskytnout reálné, spustitelné projekty, ne jen teoretické ukázky

## Jak projekty spustit

Každý projekt obsahuje vlastní README s instrukcemi.
Většina projektů vyžaduje:
- nainstalovanou Ollamu
- Docker (pro Qdrant a produkční stack)
- .NET 8 SDK

## Kontakt

Pokud vás zajímá architektura, implementace nebo spolupráce na AI projektech, rád poskytnu více informací.


