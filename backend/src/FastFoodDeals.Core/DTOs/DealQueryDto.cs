namespace FastFoodDeals.Core.DTOs;

/// <summary>
/// Filtros opcionales para la consulta de cupones activos.
/// Todos los campos son opcionales; si son null se ignoran en la query.
/// </summary>
public record DealQueryDto(
    string? Brand = null,
    string? Category = null,
    string? LocationZone = null
);
