import React from 'react';
import { Flame, MapPin } from 'lucide-react';

export function Header() {
  return (
    <header className="bg-white border-b border-gray-100 sticky top-0 z-30 shadow-xs">
      <div className="max-w-4xl mx-auto px-4 h-16 flex items-center justify-between">
        <div className="flex items-center gap-2.5">
          <div className="w-10 h-10 rounded-xl bg-linear-to-tr from-red-600 to-amber-500 flex items-center justify-center text-white shadow-sm">
            <Flame size={22} className="fill-yellow-300 stroke-red-600" />
          </div>
          <div>
            <h1 className="font-extrabold text-gray-900 tracking-tight leading-none text-lg">
              FastFood <span className="text-red-600">Deals</span>
            </h1>
            <p className="text-[11px] font-medium text-gray-400">
              Descuentos en un solo lugar
            </p>
          </div>
        </div>

        <div className="flex items-center gap-1.5 bg-gray-50 border border-gray-200 px-3 py-1.5 rounded-full text-xs font-semibold text-gray-700">
          <MapPin size={13} className="text-red-500" />
          <span>Córdoba</span>
        </div>
      </div>
    </header>
  );
}
