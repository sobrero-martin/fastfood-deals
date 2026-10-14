using FastFoodDeals.Core.DTOs;
using FastFoodDeals.Core.Entities;

namespace FastFoodDeals.Core.Interfaces;

/// <summary>
/// Contrato de acceso a datos para cupones/descuentos.
/// La implementación concreta vive en FastFoodDeals.Infrastructure.
/// </summary>
public interface IDealRepository
{
    /// <summary>
    /// Devuelve todos los cupones activos, con filtros opcionales.
    /// Ordenados por DiscountPercentage descendente.
    /// </summary>
    Task<IEnumerable<DealDto>> GetActiveDealsAsync(DealQueryDto? query = null);

    /// <summary>
    /// Devuelve un cupón por su ID, o null si no existe.
    /// </summary>
    Task<DealDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Inserta un nuevo cupón o actualiza uno existente basándose en Brand + ExternalId.
    /// Retorna true si fue insertado nuevo, false si fue actualizado.
    /// </summary>
    Task<bool> UpsertAsync(Deal deal);

    /// <summary>
    /// Marca como inactivos todos los cupones de una marca que no estén en la lista de IDs externos provistos.
    /// Usado por el scraper para desactivar ofertas que ya no existen en la fuente.
    /// </summary>
    Task DeactivateStaleDealsAsync(string brand, IEnumerable<string> activeExternalIds);
}
