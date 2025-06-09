# 📚 Minor Acts of Literary Espionage

A full-stack web application for searching books using the Open Library API. Enter a title, author, or subject — browse covers, summaries, and save your favorites to a local “To-Read” list with persistent local storage.

---

## 🧱 Tech Stack

| Layer     | Tech                            |
|-----------|---------------------------------|
| Frontend  | Angular                         |
| Backend   | ASP.NET Core Web API (C#)       |
| External  | Open Library Search & Works API |
| DevOps    | Docker + Docker Compose         |

---

## 🚀 Getting Started

### Prerequisites

- Docker
- Docker Compose

### One-liner to build & run

```bash
docker compose up --build
```

- Frontend: [http://localhost:4200/index.html](http://localhost:4200/index.html)
- Backend API: [http://localhost:5074/api/search](http://localhost:5074/api/search)
- Dev Swagger API: [http://localhost:5074/swagger/index.html](http://localhost:5074/swagger/index.html)

---

## 🗂 Project Structure

```
.
├── backend/
│   ├── BookSearchApi/         # ASP.NET Core Web API
│   ├── BookSearchLib/         # DTOs and shared models
│   └── Dockerfile             # Backend Docker config
│
├── frontend/
│   ├── src/                   # Angular app code
│   ├── Dockerfile             # Frontend Docker config
│   └── package.json           # Angular config
│
└── docker-compose.yml         # Orchestrates frontend + backend
```

---

## 📦 API

### `GET /api/search`

Query params:

- `title`, `author`, or `subject` — at least one is required

Example:

```http
GET /api/search?title=Dune
```

Returns a list of book summaries with cover images and descriptions.

---

## ✅ Features

- 🔍 Search books by title, author, or subject
- 🖼 View covers and detailed summaries
- ➕ Add books to a persistent “To-Read” list (stored locally)
- 🔄 Pagination for large result sets
- 🧪 Unit-tested C# backend logic
- 🐳 Dockerized for one-command startup

---

## 🧠 Inspiration

This project is a technical showcase

---

## 📖 License

MIT — do whatever you'd like, just don’t claim you wrote *Dune*.
