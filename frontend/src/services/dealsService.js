const API_BASE_URL = 'http://localhost:5041/api';

/**
 * Consulta la lista de cupones activos desde el backend de .NET
 */
export async function getActiveDeals() {
  try {
    const response = await fetch(`${API_BASE_URL}/deals`, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`Error al consultar cupones: ${response.status} ${response.statusText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error en dealsService.getActiveDeals:', error);
    throw error;
  }
}

/**
 * Registra un evento de telemetría cuando el usuario hace clic en "Canjear"
 */
export async function recordDealClick(dealId) {
  try {
    await fetch(`${API_BASE_URL}/deals/${dealId}/click`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      keepalive: true, // Asegura que la petición se complete aun si cambia de pestaña
    });
  } catch (error) {
    // La telemetría no debe romper la experiencia del usuario si falla
    console.warn('No se pudo registrar la telemetría de clic:', error);
  }
}
