# Backlog de User Stories - Fast Food Deals AI (MVP)
> Documento preparado para copiar y crear directamente como **Issues en GitHub**.  
> Cada historia incluye etiquetas sugeridas, descripción en formato estándar, versión mínima aceptable y criterios de aceptación en formato de checklist interactivo (`- [ ]`).

---

## Índice de Issues

| # | ID | Título de la Issue | Componente | Etiquetas |
|---|---|---|---|---|
| 1 | **US 1.1** | Actualización autónoma periódica de cupones | Ingesta | `backend`, `scraper`, `mvp` |
| 2 | **US 1.2** | Desactivación automática de ofertas vencidas | Ingesta | `backend`, `database`, `mvp` |
| 3 | **US 1.3** | Normalización y validación de datos del cupón | Ingesta | `backend`, `data-quality`, `mvp` |
| 4 | **US 2.1** | Selección inicial de zona geográfica y marcas | Onboarding | `frontend`, `ui/ux`, `mvp` |
| 5 | **US 2.2** | Selección de preferencia culinaria principal | Onboarding | `frontend`, `ui/ux`, `mvp` |
| 6 | **US 2.3** | Persistencia local de preferencias en el navegador | Onboarding | `frontend`, `storage`, `mvp` |
| 7 | **US 3.1** | Generación del Top 3 de ofertas personalizadas | Motor IA | `backend`, `ai`, `mvp` |
| 8 | **US 3.2** | Explicación persuasiva de la recomendación con IA | Motor IA | `backend`, `ai`, `frontend`, `mvp` |
| 9 | **US 3.3** | Priorización de ofertas por máximo ahorro | Motor IA | `backend`, `ai`, `mvp` |
| 10 | **US 4.1** | Comparativa visual de precios y ahorro en ARS | Ficha de Canje | `frontend`, `ui/ux`, `mvp` |
| 11 | **US 4.2** | Copiado de código promocional en un toque | Ficha de Canje | `frontend`, `ux`, `mvp` |
| 12 | **US 4.3** | Redirección directa al canal oficial de la marca | Ficha de Canje | `frontend`, `analytics`, `mvp` |
| 13 | **US 5.1** | Calificación de sugerencias en un clic (👍 / 👎) | Telemetría | `frontend`, `backend`, `mvp` |
| 14 | **US 5.2** | Registro de eventos de intención de canje | Telemetría | `backend`, `analytics`, `mvp` |
| 15 | **US 5.3** | Endpoint de métricas de satisfacción y clics | Telemetría | `backend`, `metrics`, `mvp` |

---

## Feature 1: Ingesta y Scraping Automatizado de Descuentos

### Issue #1: [US 1.1] Actualización autónoma periódica de cupones
* **Labels:** `backend`, `scraper`, `mvp`
* **Historia:**
  > **Como** administrador de la plataforma,  
  > **quiero** que los cupones de McDonald's y Burger King se extraigan automáticamente varias veces al día,  
  > **para** mantener el catálogo al día sin necesidad de cargar ofertas a mano.
* **Versión mínima aceptable:**
  Un `BackgroundService` en .NET 10 con un temporizador cada 8 horas que consulta las páginas/APIs web de promociones de McDonald's y Burger King Argentina e inserta o actualiza los registros en MySQL.
* **Criterios de aceptación:**
  - [ ] El proceso se ejecuta de forma desatendida en segundo plano cada 8 horas.
  - [ ] El fallo en la extracción de una cadena no interrumpe la extracción de la otra (manejo de excepciones aislado por marca).
  - [ ] Realiza un *Upsert* en la tabla `Deals` de MySQL para no duplicar cupones existentes (coincidencia por identificador externo o título + marca).

---

### Issue #2: [US 1.2] Desactivación automática de ofertas vencidas
* **Labels:** `backend`, `database`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** que los cupones que ya no existen en los sitios oficiales dejen de mostrarse,  
  > **para** no intentar canjear promociones que ya están vencidas.
* **Versión mínima aceptable:**
  Un campo booleano `IsActive` en la tabla `Deals` de MySQL que se marca en `false` para todos los cupones cuya fecha de expiración haya pasado o que no hayan sido detectados en la última pasada del extractor.
* **Criterios de aceptación:**
  - [ ] Los cupones no encontrados en la última ejecución del extractor pasan a `IsActive = false`.
  - [ ] Las consultas de cupones activos en la API (`GET /api/deals` y recomendador) filtran estrictamente por `IsActive == true`.
  - [ ] La fecha y hora de la última verificación queda registrada en el campo `LastScrapedAt`.

---

### Issue #3: [US 1.3] Normalización y validación de datos del cupón
* **Labels:** `backend`, `data-quality`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** ver cupones con información completa (título, foto, precio en ARS y código),  
  > **para** contar con la información necesaria antes de decidir qué comer.
* **Versión mínima aceptable:**
  El parser del extractor descarta cualquier cupón que carezca de título o precio numérico en ARS, y asigna una imagen institucional por defecto si la URL de la foto original no está disponible.
* **Criterios de aceptación:**
  - [ ] Solo se guardan ofertas con título legible y precio con descuento mayor a cero.
  - [ ] Cada cupón cuenta con una URL de imagen válida o un fallback corporativo según la marca.
  - [ ] Si el cupón no tiene código alfanumérico visible, debe contener obligatoriamente una URL de redirección directa.

---

## Feature 2: Onboarding Ultraligero y Gestión Local de Preferencias

### Issue #4: [US 2.1] Selección inicial de zona geográfica y marcas
* **Labels:** `frontend`, `ui/ux`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** elegir mi zona de Córdoba y las marcas que me interesan al entrar a la web,  
  > **para** que las promociones correspondan a locales a los que puedo acudir.
* **Versión mínima aceptable:**
  Un modal en React que aparece en la primera visita con un selector desplegable de zonas de Córdoba (ej. "Centro / Nueva Córdoba", "Norte", "Todas") y casillas de selección para McDonald's y Burger King.
* **Criterios de aceptación:**
  - [ ] El modal se despliega automáticamente si no existen preferencias previas en el navegador.
  - [ ] Requiere que el usuario seleccione al menos una marca para habilitar el botón de continuar.
  - [ ] La zona y marcas elegidas se almacenan en el estado de React y en `localStorage`.

---

### Issue #5: [US 2.2] Selección de preferencia culinaria principal
* **Labels:** `frontend`, `ui/ux`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** indicar mi preferencia de comida (carne, pollo, vegetariano o máximo ahorro),  
  > **para** que el recomendador priorice platos afines a lo que tengo ganas de comer.
* **Versión mínima aceptable:**
  Un selector de opción única con 4 botones claros en el modal de onboarding: "Carne", "Pollo", "Vegetariano" y "Máximo Ahorro".
* **Criterios de aceptación:**
  - [ ] El usuario debe seleccionar exactamente una opción culinaria antes de finalizar el onboarding.
  - [ ] El valor seleccionado se persiste en `localStorage` bajo la clave `primaryPreference`.
  - [ ] La selección se envía como parámetro en la solicitud de recomendación hacia la API de .NET.

---

### Issue #6: [US 2.3] Persistencia local de preferencias en el navegador
* **Labels:** `frontend`, `storage`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** que mis preferencias queden guardadas en mi navegador sin tener que crear una cuenta con contraseña,  
  > **para** ver mis recomendaciones de inmediato cada vez que vuelva a ingresar.
* **Versión mínima aceptable:**
  Comprobación al iniciar la app: si existe el objeto `user_preferences` en `localStorage`, se omite el modal y se consultan directamente las recomendaciones.
* **Criterios de aceptación:**
  - [ ] Si `localStorage` contiene preferencias válidas, el usuario ingresa directo al feed de recomendaciones.
  - [ ] Existe un botón accesible ("Editar preferencias") en la barra superior para modificar o reiniciar los datos guardados en cualquier momento.
  - [ ] En ningún momento se solicita email, nombre ni contraseña.

---

## Feature 3: Motor de Recomendación y Justificación con IA

### Issue #7: [US 3.1] Generación del Top 3 de ofertas personalizadas
* **Labels:** `backend`, `ai`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** ver un listado reducido de las 3 mejores ofertas seleccionadas para mí,  
  > **para** tomar una decisión rápida sin abrumarme revisando decenas de cupones.
* **Versión mínima aceptable:**
  El endpoint `POST /api/recommendations` procesa las preferencias del usuario, consulta a la API de Gemini con el catálogo activo y devuelve un array con exactamente 3 cupones recomendados.
* **Criterios de aceptación:**
  - [ ] La API retorna una lista con un máximo estricto de 3 cupones.
  - [ ] Todos los cupones devueltos pertenecen a las marcas seleccionadas por el usuario y tienen `IsActive == true`.
  - [ ] La respuesta tarda menos de 3 segundos en renderizarse en el frontend.

---

### Issue #8: [US 3.2] Explicación persuasiva de la recomendación con IA
* **Labels:** `backend`, `ai`, `frontend`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** leer una breve justificación generada por IA en cada cupón recomendado,  
  > **para** entender por qué esa opción me conviene o maximiza mi ahorro.
* **Versión mínima aceptable:**
  Cada objeto de recomendación incluye una propiedad `aiReasoning` (texto de 1 o 2 oraciones) generada por Gemini y mostrada en una caja destacada en la tarjeta del cupón.
* **Criterios de aceptación:**
  - [ ] Cada una de las 3 ofertas incluye un texto `aiReasoning` no vacío generado por el LLM.
  - [ ] La justificación hace referencia explícita a la preferencia culinaria del usuario o al ahorro obtenido.
  - [ ] El texto se renderiza visualmente diferenciado con un ícono (ej. 💡 *"Por qué te lo recomendamos"*).

---

### Issue #9: [US 3.3] Priorización de ofertas por máximo ahorro
* **Labels:** `backend`, `ai`, `mvp`
* **Historia:**
  > **Como** consumidor con presupuesto acotado,  
  > **quiero** que la IA priorice las opciones con mayor porcentaje de descuento si elijo la opción de "máximo ahorro",  
  > **para** gastar la menor cantidad de dinero posible.
* **Versión mínima aceptable:**
  Cuando `primaryPreference` es `max_saving`, el prompt instruye a Gemini a seleccionar las ofertas con mayor `DiscountPercentage` y menor `DiscountPrice` dentro de MySQL.
* **Criterios de aceptación:**
  - [ ] Los cupones recomendados presentan los mayores porcentajes de descuento relativos del catálogo activo.
  - [ ] La justificación de la IA destaca explícitamente el porcentaje de descuento o el bajo precio en ARS.
  - [ ] Ningún combo de categoría prémium de alto costo es priorizado bajo este modo.

---

## Feature 4: Ficha de Canje y Redirección Directa

### Issue #10: [US 4.1] Comparativa visual de precios y ahorro en ARS
* **Labels:** `frontend`, `ui/ux`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** ver en la tarjeta el precio regular tachado, el precio final con descuento y el porcentaje de ahorro,  
  > **para** saber con precisión cuánto dinero me estoy ahorrando.
* **Versión mínima aceptable:**
  Una tarjeta en React que muestre el precio regular tachado, el precio final en ARS con tipografía destacada y una etiqueta con el porcentaje de descuento redondeado.
* **Criterios de aceptación:**
  - [ ] Los precios se formatean en moneda local argentina (ej. `$ 4.200`).
  - [ ] El porcentaje de descuento se visualiza como un número entero (ej. `-35% OFF`).
  - [ ] Si el precio original no está disponible, se muestra únicamente el precio final sin aplicar tachados incorrectos.

---

### Issue #11: [US 4.2] Copiado de código promocional en un toque
* **Labels:** `frontend`, `ux`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** copiar el código de descuento al portapapeles con un solo toque,  
  > **para** usarlo fácilmente en el checkout o en la caja.
* **Versión mínima aceptable:**
  Un botón que ejecuta `navigator.clipboard.writeText(couponCode)` y cambia temporalmente su texto a *"¡Copiado! ✓"* durante 2 segundos.
* **Criterios de aceptación:**
  - [ ] Al tocar el botón, el código se copia al portapapeles del dispositivo.
  - [ ] El botón muestra retroalimentación visual inmediata del éxito de la copia.
  - [ ] Si el cupón no requiere código físico, el botón indica *"Ir a la promo"* en lugar de copiar.

---

### Issue #12: [US 4.3] Redirección directa al canal oficial de la marca
* **Labels:** `frontend`, `analytics`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** hacer clic en un enlace que me lleve directamente al sitio de la marca correspondiente,  
  > **para** concretar mi pedido sin tener que buscar la promoción por mi cuenta.
* **Versión mínima aceptable:**
  Un botón de acción principal que abre la `RedemptionUrl` de la cadena en una nueva pestaña del navegador mediante `target="_blank"`.
* **Criterios de aceptación:**
  - [ ] El enlace redirige a la URL oficial de la oferta de McDonald's o Burger King.
  - [ ] La apertura se realiza en una pestaña nueva sin cerrar la aplicación de recomendaciones.
  - [ ] El clic ejecuta en paralelo una llamada asíncrona a la API de telemetría sin retrasar la navegación.

---

## Feature 5: Telemetría de Canje y Calificación de Sugerencias

### Issue #13: [US 5.1] Calificación de sugerencias en un clic (👍 / 👎)
* **Labels:** `frontend`, `backend`, `mvp`
* **Historia:**
  > **Como** consumidor,  
  > **quiero** calificar con un pulgar arriba (👍) o abajo (👎) si la recomendación me sirvió,  
  > **para** dar feedback sobre la sugerencia en un solo clic sin completar encuestas molestas.
* **Versión mínima aceptable:**
  Dos botones (👍 y 👎) al pie de las recomendaciones que envían un `POST` a `/api/feedback` y muestran un mensaje breve de agradecimiento.
* **Criterios de aceptación:**
  - [ ] Al pulsar un botón, se envía el payload `{ isPositive: true/false }` al backend.
  - [ ] Los botones se deshabilitan tras el clic para evitar votos repetidos en la misma sesión.
  - [ ] Se muestra un mensaje visual de confirmación (*"¡Gracias por tu opinión!"*).

---

### Issue #14: [US 5.2] Registro de eventos de intención de canje
* **Labels:** `backend`, `analytics`, `mvp`
* **Historia:**
  > **Como** administrador de la plataforma,  
  > **quiero** registrar cada clic en el botón de canje de un cupón,  
  > **para** medir cuántos usuarios realmente muestran intención de compra.
* **Versión mínima aceptable:**
  Un endpoint `POST /api/deals/{id}/click` en .NET 10 que inserte un registro en una tabla `DealClicks` en MySQL con el ID del cupón y la fecha/hora UTC.
* **Criterios de aceptación:**
  - [ ] Cada clic en *"Copiar código / Ir a la web"* envía una petición al endpoint correspondiente.
  - [ ] El endpoint responde con código de estado `204 No Content` de manera ultrarrápida.
  - [ ] El registro queda almacenado en MySQL con `DealId` y `TimestampUtc`.

---

### Issue #15: [US 5.3] Endpoint de métricas de satisfacción y clics
* **Labels:** `backend`, `metrics`, `mvp`
* **Historia:**
  > **Como** administrador de la plataforma,  
  > **quiero** consultar el total de valoraciones positivas y negativas recibidas,  
  > **para** evaluar si las recomendaciones de la IA están siendo útiles para los usuarios.
* **Versión mínima aceptable:**
  Un endpoint básico `GET /api/feedback/summary` que consulte MySQL y devuelva la cantidad acumulada de votos positivos, negativos y total de clics de canje.
* **Criterios de aceptación:**
  - [ ] La API retorna un JSON con `{ totalPositive: int, totalNegative: int, totalClicks: int }`.
  - [ ] El cálculo se obtiene en tiempo real directamente de la base de datos MySQL.
  - [ ] Permite verificar si se está alcanzando el KPI de $\ge 70\%$ de feedback positivo establecido en el MVP.
