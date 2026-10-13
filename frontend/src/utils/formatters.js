/**
 * Formatea un número al estándar monetario de Argentina (ARS)
 * Ejemplo: 4200 -> "$ 4.200"
 */
export function formatCurrency(amount) {
  if (amount === undefined || amount === null || isNaN(amount)) {
    return '$ 0';
  }

  return new Intl.NumberFormat('es-AR', {
    style: 'currency',
    currency: 'ARS',
    maximumFractionDigits: 0,
  }).format(amount);
}

/**
 * Formatea un porcentaje de descuento
 * Ejemplo: 35.4 -> "-35% OFF"
 */
export function formatDiscount(percentage) {
  if (!percentage) return '';
  return `-${Math.round(percentage)}% OFF`;
}
