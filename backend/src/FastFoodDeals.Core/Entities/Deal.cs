namespace FastFoodDeals.Core.Entities;

public class Deal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// ID o código de origen del cupón en la plataforma de la cadena (para evitar duplicados).
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Cadena de comida rápida: "McDonald's" | "Burger King"
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Precio regular tachado en ARS
    /// </summary>
    public decimal OriginalPrice { get; set; }

    /// <summary>
    /// Precio final con descuento en ARS
    /// </summary>
    public decimal DiscountPrice { get; set; }

    /// <summary>
    /// Porcentaje de ahorro calculado (ej: 35.5)
    /// </summary>
    public double DiscountPercentage { get; set; }

    /// <summary>
    /// Categoría culinaria: "burger_beef" | "chicken" | "veggie" | "dessert" | "cafe" | "general"
    /// </summary>
    public string Category { get; set; } = "general";

    /// <summary>
    /// Código alfanumérico para canjear (si aplica)
    /// </summary>
    public string? CouponCode { get; set; }

    /// <summary>
    /// URL directa a la web/app de la marca para canjear
    /// </summary>
    public string RedemptionUrl { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Zona geográfica en Córdoba: "cordoba_centro", "cordoba_norte", "cordoba_all"
    /// </summary>
    public string LocationZone { get; set; } = "cordoba_all";

    public DateTime? ExpiresAt { get; set; }

    public DateTime LastScrapedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
