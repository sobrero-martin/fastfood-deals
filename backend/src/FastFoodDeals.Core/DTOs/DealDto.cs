namespace FastFoodDeals.Core.DTOs;

/// <summary>
/// Objeto de transferencia de datos de un cupón hacia el cliente (frontend React).
/// Expone únicamente los campos necesarios para la vista, omitiendo detalles internos como LastScrapedAt.
/// </summary>
public record DealDto(
    Guid Id,
    string Brand,
    string Title,
    string Description,
    decimal OriginalPrice,
    decimal DiscountPrice,
    double DiscountPercentage,
    string Category,
    string? CouponCode,
    string RedemptionUrl,
    string ImageUrl,
    string LocationZone,
    DateTime? ExpiresAt
);
