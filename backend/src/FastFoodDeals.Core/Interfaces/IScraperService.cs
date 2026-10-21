namespace FastFoodDeals.Core.Interfaces;

/// <summary>
/// Contrato común para los scrapers de diferentes cadenas de comida rápida.
/// </summary>
public interface IScraperService
{
    /// <summary>
    /// Nombre de la marca que extrae este scraper (ej. "McDonald's", "Burger King").
    /// </summary>
    string BrandName { get; }

    /// <summary>
    /// Ejecuta la extracción de ofertas y las sincroniza en la base de datos.
    /// Retorna la cantidad de cupones/ofertas procesadas.
    /// </summary>
    Task<int> ScrapeAndSyncAsync();
}
