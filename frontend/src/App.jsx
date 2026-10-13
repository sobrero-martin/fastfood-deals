import React, { useEffect, useState } from 'react';
import { Header } from './components/layout/Header';
import { DealCard } from './features/deals/DealCard';
import { getActiveDeals } from './services/dealsService';
import { RefreshCw, AlertCircle, Sparkles } from 'lucide-react';

export default function App() {
  const [deals, setDeals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchDeals = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getActiveDeals();
      setDeals(data);
    } catch (err) {
      setError(
        'No se pudo conectar con el servidor. Asegúrate de que el backend de .NET esté corriendo.'
      );
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDeals();
  }, []);

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col font-sans">
      <Header />

      <main className="flex-1 max-w-4xl w-full mx-auto px-4 py-6">
        {/* Banner de bienvenida / Introducción */}
        <div className="mb-6 bg-linear-to-r from-red-600 to-amber-600 rounded-3xl p-6 text-white shadow-sm flex items-center justify-between">
          <div>
            <div className="flex items-center gap-1.5 text-yellow-300 text-xs font-bold uppercase tracking-wider mb-1">
              <Sparkles size={14} />
              <span>Córdoba Piloto</span>
            </div>
            <h2 className="text-xl sm:text-2xl font-black tracking-tight leading-tight">
              Los mejores descuentos de hoy
            </h2>
            <p className="text-xs sm:text-sm text-red-100 mt-1 max-w-md">
              Centralizamos promociones de McDonald's y Burger King para que ahorres sin abrir múltiples aplicaciones.
            </p>
          </div>
        </div>

        {/* Barra de estado / recarga */}
        <div className="flex items-center justify-between mb-4">
          <p className="text-sm font-semibold text-gray-700">
            {loading ? (
              'Cargando ofertas...'
            ) : (
              <span>
                Mostrando <strong className="text-gray-900">{deals.length}</strong> cupones activos
              </span>
            )}
          </p>

          <button
            type="button"
            onClick={fetchDeals}
            disabled={loading}
            className="flex items-center gap-1.5 text-xs font-semibold text-gray-600 hover:text-gray-900 bg-white border border-gray-200 px-3 py-1.5 rounded-lg shadow-2xs hover:bg-gray-50 active:scale-95 transition-all disabled:opacity-50 cursor-pointer"
          >
            <RefreshCw size={13} className={loading ? 'animate-spin' : ''} />
            <span>Actualizar</span>
          </button>
        </div>

        {/* Estado: Error de conexión */}
        {error && (
          <div className="bg-red-50 border border-red-200 rounded-2xl p-5 text-red-800 flex items-start gap-3 my-6">
            <AlertCircle size={20} className="text-red-600 shrink-0 mt-0.5" />
            <div>
              <h4 className="font-bold text-sm">Error de conexión</h4>
              <p className="text-xs text-red-700 mt-1">{error}</p>
              <button
                type="button"
                onClick={fetchDeals}
                className="mt-3 text-xs font-bold bg-red-600 text-white px-3 py-1.5 rounded-lg hover:bg-red-700 active:scale-95 transition-all"
              >
                Reintentar
              </button>
            </div>
          </div>
        )}

        {/* Estado: Cargando (Skeletons) */}
        {loading && (
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
            {[1, 2, 3, 4].map((i) => (
              <div
                key={i}
                className="bg-white rounded-2xl border border-gray-100 overflow-hidden shadow-2xs animate-pulse"
              >
                <div className="h-48 bg-gray-200" />
                <div className="p-5 space-y-3">
                  <div className="h-5 bg-gray-200 rounded-md w-3/4" />
                  <div className="h-4 bg-gray-100 rounded-md w-full" />
                  <div className="h-6 bg-gray-200 rounded-md w-1/3 pt-2" />
                  <div className="h-10 bg-gray-100 rounded-xl w-full" />
                </div>
              </div>
            ))}
          </div>
        )}

        {/* Estado: Lista de cupones */}
        {!loading && !error && deals.length > 0 && (
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
            {deals.map((deal) => (
              <DealCard key={deal.id} deal={deal} />
            ))}
          </div>
        )}

        {/* Estado: Sin cupones */}
        {!loading && !error && deals.length === 0 && (
          <div className="text-center py-16 bg-white rounded-2xl border border-gray-100 p-8">
            <p className="text-gray-500 text-sm">
              No hay cupones activos en este momento.
            </p>
          </div>
        )}
      </main>

      <footer className="border-t border-gray-100 bg-white py-6 mt-12 text-center text-xs text-gray-400">
        <p>Fast Food Deals AI • Córdoba, Argentina</p>
      </footer>
    </div>
  );
}
