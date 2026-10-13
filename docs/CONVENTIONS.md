# Guía de Convenciones y Buenas Prácticas
## Fast Food Deals AI (Córdoba, Argentina)

> Este documento establece los estándares de diseño, arquitectura, nomenclatura y estilo para mantener la consistencia técnica a lo largo de todo el desarrollo del proyecto.

---

## 1. Principios Generales

1. **Simplicidad ante todo (KISS & YAGNI):** No crear abstracciones complejas para problemas que aún no existen. Evitar patrones pesados si un servicio directo resuelve el problema.
2. **Respeto estricto del MVP:** No introducir autenticación con contraseñas, pasarelas de pago, bases de datos adicionales ni dependencias fuera del alcance delimitado en `docs/MVP.md`.
3. **Flujo Local-First:** Todo cambio debe probarse y validarse en el entorno local antes de realizar commits o pushes al repositorio remoto.

---

## 2. Backend (.NET 10 / C#)

### 2.1. Nomenclatura y Casing
| Elemento | Convención | Ejemplo |
| :--- | :--- | :--- |
| **Namespaces** | `PascalCase` | `FastFoodDeals.Core.Entities` |
| **Clases y Records** | `PascalCase` | `Deal`, `DealRepository`, `UserPreferencesDto` |
| **Interfaces** | `IPascalCase` (prefijo `I`) | `IDealRepository`, `IScraperService` |
| **Métodos y Propiedades** | `PascalCase` | `GetActiveDealsAsync()`, `DiscountPrice` |
| **Variables locales y parámetros** | `camelCase` | `dealId`, `connectionString`, `context` |
| **Campos privados** | `_camelCase` (guion bajo) | `_context`, `_logger`, `_httpClient` |
| **Constantes** | `PascalCase` | `DefaultLocationZone`, `MaxRecommendations` |

### 2.2. Base de Datos (MySQL + Entity Framework Core)
* **Nombres de tablas:** Plural en `PascalCase` (`Deals`, `DealClicks`, `FeedbackEvents`).
* **Identificadores (IDs):** Siempre `Guid` generado con `Guid.NewGuid()`.
* **Moneda y Decimales:**
  * Precios en pesos argentinos (`ARS`).
  * Especificar siempre precisión en Fluent API o Data Annotations: `HasPrecision(10, 2)` o `[Column(TypeName = "decimal(10,2)")]`.
* **Manejo de Fechas:**
  * **Regla estricta:** Todas las fechas almacenadas en la base de datos deben ser **UTC** (`DateTime.UtcNow`).
  * Los campos de fecha deben terminar con el sufijo `Utc` o `At` (ej. `LastScrapedAt`, `ExpiresAt`, `CreatedAtUtc`).
* **Booleans:** Prefijos afirmativos claros (`IsActive`, `IsPositive`).

### 2.3. API REST y Controladores
* **Rutas de endpoints:** En minúsculas, kebab-case y sustantivos en plural:
  * `GET /api/deals`
  * `POST /api/recommendations`
  * `POST /api/deals/{id}/click`
  * `POST /api/feedback`
* **Serialización JSON:**
  * **Salida obligatoria en `camelCase`:** La configuración de ASP.NET Core debe serializar hacia el frontend en camelCase (`originalPrice`, `discountPrice`, `aiReasoning`), nunca en PascalCase.
* **Códigos de estado HTTP:**
  * `200 OK`: Consultas exitosas con cuerpo.
  * `201 Created`: Recursos creados.
  * `204 No Content`: Acciones exitosas sin retorno de cuerpo (ej. clics de telemetría).
  * `400 Bad Request`: Datos de entrada inválidos.
  * `404 Not Found`: Recurso inexistente.

---

## 3. Frontend (React 19 + JavaScript + Tailwind CSS)

### 3.1. Nomenclatura y Casing
| Elemento | Extensión | Convención | Ejemplo |
| :--- | :--- | :--- | :--- |
| **Componentes React** | `.jsx` | `PascalCase` | `DealCard.jsx`, `Header.jsx`, `OnboardingModal.jsx` |
| **Hooks personalizados** | `.js` | `camelCase` (`use...`) | `usePreferences.js`, `useDeals.js` |
| **Servicios / API** | `.js` | `camelCase` (`...Service`) | `dealsService.js`, `recommendationService.js` |
| **Utilidades y Helpers** | `.js` | `camelCase` | `formatters.js`, `storage.js` |
| **Variables y Funciones** | N/A | `camelCase` | `handleCopyCode`, `isLoading`, `activeFilter` |
| **Constantes globales** | N/A | `UPPER_SNAKE_CASE` | `API_BASE_URL`, `DEFAULT_ZONE` |

### 3.2. Estructura de Componentes
* **Modularidad por Features:**
  * `src/components/ui/`: Componentes reutilizables sin lógica de negocio (botones, modales, badges genéricos).
  * `src/components/layout/`: Estructura general de página (header, footer, contenedores).
  * `src/features/{feature}/`: Componentes acoplados al dominio (`features/deals/`, `features/onboarding/`, `features/recommendations/`).
* **Separación de responsabilidades:**
  * Los componentes de UI reciben datos por *props* y emiten eventos (`onSelect`, `onClick`).
  * Las llamadas `fetch` / HTTP se encapsulan exclusivamente en `src/services/`, nunca directamente en el cuerpo del JSX.

### 3.3. Estilos y Tailwind CSS
* **Filosofía Mobile-First:** Diseñar siempre para pantallas de celular primero (`px-4 py-5 flex-col`), y añadir variantes de escritorio con prefijos (`sm:grid-cols-2`, `md:flex-row`).
* **Paleta de Identidad Visual:**
  * **McDonald's:** Rojo corporativo (`bg-red-600 text-yellow-300`).
  * **Burger King:** Naranja/fuego (`bg-amber-600 text-white`).
  * **Ahorro y Descuentos:** Verde esmeralda (`text-emerald-600`, `bg-emerald-50`, `border-emerald-200`).
  * **Fondos y Neutros:** Gris suave (`bg-gray-50`) con tarjetas blancas (`bg-white border-gray-100 shadow-sm`).
* **Formateo de Precios:**
  * No concatenar signos `$` a mano en el JSX. Utilizar siempre `formatCurrency(amount)` de `formatters.js` para garantizar `$ 4.200`.

### 3.4. Persistencia en el Navegador
* Las claves de `localStorage` deben tener el prefijo del proyecto en `snake_case`:
  * `fastfood_preferences`: Guarda el JSON con `{ zone, brands, primaryPreference }`.

---

## 4. Integración con Inteligencia Artificial (Google Gemini)

1. **Formato Estricto JSON:** Todo prompt enviado a Gemini debe requerir salida estructurada (`Structured Outputs` / JSON Schema) con:
   ```json
   {
     "recommendations": [
       { "dealId": "guid", "reasoning": "Texto explicativo" }
     ]
   }
   ```
2. **Tono de la Justificación (`aiReasoning`):**
   * Tono: Cercano, persuasivo, directo.
   * Longitud: Máximo 25 palabras (1 o 2 oraciones).
   * Contenido: Debe mencionar el % de ahorro o la relación precio/calidad justificando por qué coincide con el gusto del usuario.
3. **Resiliencia y Fallback:**
   * Si la llamada a la IA demora más de 3 segundos o arroja error, el backend debe retornar los 3 cupones activos con mayor porcentaje de descuento con un texto predeterminado (*"Opción destacada con el mayor ahorro del día"*).

---

## 5. Control de Versiones y Git

* **Commits Semánticos (Conventional Commits):**
  * `feat:` Nueva funcionalidad o componente.
  * `fix:` Corrección de errores.
  * `docs:` Cambios en documentación (`.md`).
  * `refactor:` Mejoras en código sin cambio de comportamiento.
  * `style:` Ajustes puramente visuales o de formato.
* **Vinculación con GitHub Issues:**
  * Incluir la referencia de la tarea en el commit:
    * `feat(frontend): crear tarjeta de cupones con copiado de código (Refs #17, #18)`
    * `feat(backend): implementar endpoint de feedback (Closes #20)`
* **Regla de Ejecución:** No realizar commits ni pushes automáticos sin confirmación y prueba previa en el entorno local.
