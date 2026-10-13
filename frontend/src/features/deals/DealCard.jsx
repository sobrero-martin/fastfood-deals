import React, { useState } from 'react';
import { Copy, Check, ExternalLink, Tag } from 'lucide-react';
import { formatCurrency, formatDiscount } from '../../utils/formatters';

export function DealCard({ deal }) {
  const [copied, setCopied] = useState(false);

  const handleCopyCode = async () => {
    if (!deal.couponCode) return;

    try {
      await navigator.clipboard.writeText(deal.couponCode);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error('No se pudo copiar el código:', err);
    }
  };

  const isMcDonalds = deal.brand.toLowerCase().includes('mcdonald');

  return (
    <div className="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden flex flex-col hover:shadow-md transition-shadow duration-200">
      {/* Cabecera de la tarjeta con imagen y badges */}
      <div className="relative h-48 bg-gray-50 flex items-center justify-center overflow-hidden">
        <img
          src={deal.imageUrl}
          alt={deal.title}
          className="w-full h-full object-cover object-center"
          onError={(e) => {
            // Fallback genérico si la imagen externa no carga
            e.target.onerror = null;
            e.target.src = 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500&auto=format&fit=crop&q=60';
          }}
        />

        {/* Badge de la Marca */}
        <span
          className={`absolute top-3 left-3 px-3 py-1 rounded-full text-xs font-bold tracking-wide uppercase shadow-sm ${
            isMcDonalds
              ? 'bg-red-600 text-yellow-300'
              : 'bg-amber-600 text-white'
          }`}
        >
          {deal.brand}
        </span>

        {/* Badge de Porcentaje de Descuento */}
        {deal.discountPercentage > 0 && (
          <span className="absolute top-3 right-3 bg-emerald-600 text-white px-2.5 py-1 rounded-full text-xs font-extrabold tracking-tight shadow-sm flex items-center gap-1">
            <Tag size={12} />
            {formatDiscount(deal.discountPercentage)}
          </span>
        )}
      </div>

      {/* Contenido principal */}
      <div className="p-5 flex-1 flex flex-col justify-between">
        <div>
          <h3 className="text-lg font-bold text-gray-900 leading-snug mb-1">
            {deal.title}
          </h3>
          <p className="text-sm text-gray-500 line-clamp-2 mb-4">
            {deal.description}
          </p>
        </div>

        <div>
          {/* Bloque comparativo de precios */}
          <div className="flex items-baseline gap-2 mb-4">
            <span className="text-2xl font-black text-gray-900">
              {formatCurrency(deal.discountPrice)}
            </span>
            {deal.originalPrice > deal.discountPrice && (
              <span className="text-sm text-gray-400 line-through font-medium">
                {formatCurrency(deal.originalPrice)}
              </span>
            )}
          </div>

          {/* Botones de acción */}
          <div className="flex items-center gap-2">
            {deal.couponCode ? (
              <button
                type="button"
                onClick={handleCopyCode}
                className={`flex-1 py-2.5 px-3 rounded-xl font-semibold text-xs transition-all duration-150 flex items-center justify-center gap-1.5 border ${
                  copied
                    ? 'bg-emerald-50 text-emerald-700 border-emerald-300'
                    : 'bg-gray-50 text-gray-800 border-gray-200 hover:bg-gray-100 active:scale-95'
                }`}
              >
                {copied ? (
                  <>
                    <Check size={14} className="text-emerald-600" />
                    <span>¡Copiado!</span>
                  </>
                ) : (
                  <>
                    <Copy size={14} className="text-gray-500" />
                    <span className="font-mono">{deal.couponCode}</span>
                  </>
                )}
              </button>
            ) : null}

            {deal.redemptionUrl ? (
              <a
                href={deal.redemptionUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="flex-1 py-2.5 px-3 rounded-xl font-bold text-xs bg-red-600 hover:bg-red-700 text-white flex items-center justify-center gap-1.5 shadow-sm transition-all active:scale-95 text-center"
              >
                <span>Canjear</span>
                <ExternalLink size={13} />
              </a>
            ) : null}
          </div>
        </div>
      </div>
    </div>
  );
}
