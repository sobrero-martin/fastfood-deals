# Fast Food Deals AI 🍔🍟
> **Recomendador inteligente de descuentos y cupones de comida rápida para la ciudad de Córdoba, Argentina.**

---

## 📌 Descripción del Proyecto

Aplicación web móvil (*mobile-first*) que centraliza automáticamente los cupones vigentes de las principales cadenas de comida rápida en Córdoba (McDonald's y Burger King) y utiliza Inteligencia Artificial (Google Gemini) para recomendar de forma personalizada las **3 mejores ofertas** según la zona, preferencias culinarias y presupuesto del usuario, explicando la razón del ahorro en lenguaje natural.

---

## 🛠️ Stack Tecnológico

* **Backend:** [.NET 10](https://dotnet.microsoft.com/) (ASP.NET Core Web API)
  * **Scraping e Ingesta:** `HttpClient` (APIs web privadas) + `Microsoft.Playwright` (C#)
  * **Base de Datos:** MySQL 8.0+ con Entity Framework Core (`Pomelo.EntityFrameworkCore.MySql`)
  * **Motor IA:** Google Gemini API (Salidas estructuradas en JSON)
* **Frontend:** [React](https://react.dev/) (Vite + TypeScript + Tailwind CSS)
  * **Persistencia:** `localStorage` (sin autenticación obligatoria para el MVP)
  * **Diseño:** Mobile-first con `lucide-react`

---

## 📂 Estructura del Repositorio

```text
fastfood-deals/
├── docs/
│   ├── MVP.md              # Especificación técnica y arquitectónica detallada
│   └── USER_STORIES.md     # Backlog inicial de historias de usuario
├── backend/                # Solución .NET 10 (ASP.NET Core Web API)
├── frontend/               # Aplicación React + Vite + Tailwind CSS
├── .gitignore
└── README.md
```

---

## 📚 Documentación

* 📄 **[Especificación Técnica y de Producto (MVP)](docs/MVP.md):** Arquitectura, modelos de datos, endpoints de API y alcance detallado.
* 📋 **[Backlog de User Stories](docs/USER_STORIES.md):** Historias de usuario, versiones mínimas aceptables y criterios de aceptación.
* 🎯 **[GitHub Issues](https://github.com/sobrero-martin/fastfood-deals/issues):** Tablero de tareas activo para el desarrollo del MVP.

---

## 🚀 Estado del Proyecto
* **Fase actual:** Configuración inicial del proyecto y arquitectura base.
* **Próximo hito:** Implementación del pipeline de ingesta / scraping automatizado (.NET 10 + MySQL).
