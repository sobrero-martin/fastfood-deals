using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using FastFoodDeals.Core.Entities;
using FastFoodDeals.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FastFoodDeals.Infrastructure.Scrapers;

public class McDonaldsScraper : IScraperService
{
    private readonly HttpClient _httpClient;
    private readonly IDealRepository _dealRepository;
    private readonly ILogger<McDonaldsScraper> _logger;

    // URL descubierta mediante DevTools de McDonald's Argentina
    private const string ApiUrl = "https://api-mcd-ecommerce-ar.appmcdonalds.com/catalog/menu/lite?restaurant=61979fe6c6ad5f3624abd4e8&area=MOP&outdaypart=true";

    // Base URL oficial de Pide y Retira para la sucursal de Córdoba
    private const string BaseOrderUrl = "https://www.mcdonalds.com.ar/restaurantes/cordoba/nunez-mujer-urbana-anc/pedidos/pedi-y-retira";

    public string BrandName => "McDonald's";

    public McDonaldsScraper(
        HttpClient httpClient,
        IDealRepository dealRepository,
        ILogger<McDonaldsScraper> logger)
    {
        _httpClient = httpClient;
        _dealRepository = dealRepository;
        _logger = logger;
    }

    public async Task<int> ScrapeAndSyncAsync()
    {
        _logger.LogInformation("Iniciando extracción de ofertas de McDonald's Argentina...");

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var activeExternalIds = new List<string>();
            var dealsProcessed = 0;

            foreach (var category in doc.RootElement.EnumerateArray())
            {
                if (!category.TryGetProperty("products", out var products) || products.ValueKind != JsonValueKind.Array)
                    continue;

                // Título y slug de la categoría real (ej: "Ofertas", "Ofertas McCafé", "Ofertas Desayuno")
                var categoryTitle = category.TryGetProperty("title", out var catProp) ? catProp.GetString() ?? "Ofertas" : "Ofertas";
                var categorySlug = GenerateSlug(categoryTitle);

                foreach (var product in products.EnumerateArray())
                {
                    if (!IsPromotion(product, out var discountPct))
                        continue;

                    var externalId = product.GetProperty("id").GetString() ?? Guid.NewGuid().ToString();
                    var name = product.GetProperty("name").GetString() ?? "Promoción McDonald's";
                    var description = product.TryGetProperty("description", out var descProp) ? descProp.GetString() ?? "" : "";
                    var imageUrl = product.TryGetProperty("imageUrl", out var imgProp) ? imgProp.GetString() ?? "" : "";

                    // Precio en centavos (ej: 1999900 = $19.999,00)
                    decimal discountPrice = 0;
                    if (product.TryGetProperty("price", out var priceProp) && priceProp.TryGetProperty("amount", out var amountProp))
                    {
                        discountPrice = amountProp.GetInt64() / 100m;
                    }

                    if (discountPrice <= 0)
                        continue;

                    // Calcular precio original estimado
                    decimal originalPrice = discountPrice;
                    if (discountPct > 0 && discountPct < 100)
                    {
                        originalPrice = Math.Round(discountPrice / (1 - (decimal)(discountPct / 100.0)), 2);
                    }
                    else
                    {
                        discountPct = 25;
                        originalPrice = Math.Round(discountPrice * 1.33m, 2);
                    }

                    // Generar enlace directo con la categoría dinámica exacta y el slug del producto
                    var productSlug = GenerateSlug(name);
                    var directUrl = $"{BaseOrderUrl}/{categorySlug}/{productSlug}";

                    var deal = new Deal
                    {
                        ExternalId = externalId,
                        Brand = BrandName,
                        Title = name,
                        Description = string.IsNullOrWhiteSpace(description) ? "Promoción exclusiva en locales de McDonald's." : description,
                        OriginalPrice = originalPrice,
                        DiscountPrice = discountPrice,
                        DiscountPercentage = Math.Round(discountPct, 1),
                        Category = DetermineCategory(name),
                        CouponCode = null,
                        RedemptionUrl = directUrl,
                        ImageUrl = imageUrl,
                        LocationZone = "cordoba_all",
                        ExpiresAt = DateTime.UtcNow.AddDays(7),
                        LastScrapedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    await _dealRepository.UpsertAsync(deal);
                    activeExternalIds.Add(externalId);
                    dealsProcessed++;
                }
            }

            _logger.LogInformation("Extracción finalizada. Se procesaron {Count} ofertas de McDonald's con categorías dinámicas.", dealsProcessed);
            return dealsProcessed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la extracción de McDonald's");
            throw;
        }
    }

    /// <summary>
    /// Convierte cualquier texto a slug URL limpio y normalizado.
    /// Ej: "Ofertas McCafé" -> "ofertas-mccafe"
    /// Ej: "2do Iced Coffe 50%OFF" -> "2do-iced-coffe-50off"
    /// </summary>
    private static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        // Remover acentos y caracteres diacríticos (é -> e, á -> a, etc.)
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        var cleanText = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

        // Reemplazar caracteres no alfanuméricos por guiones
        cleanText = Regex.Replace(cleanText, @"[^a-z0-9\s-]", "");
        // Reemplazar múltiples espacios o guiones por uno solo
        cleanText = Regex.Replace(cleanText, @"[\s-]+", " ").Trim();
        cleanText = Regex.Replace(cleanText, @"\s", "-");

        return cleanText;
    }

    private static bool IsPromotion(JsonElement product, out double discountPercentage)
    {
        discountPercentage = 0;
        var name = product.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";

        var isOffer = false;
        if (product.TryGetProperty("persuasiveLabel", out var label) && label.ValueKind == JsonValueKind.Object)
        {
            if (label.TryGetProperty("type", out var type) && type.GetString() == "OFFER")
            {
                isOffer = true;
            }
        }

        var lowerName = name.ToLowerInvariant();
        var hasPromoKeyword = lowerName.Contains("off") ||
                              lowerName.Contains("promo") ||
                              lowerName.Contains("3x2") ||
                              lowerName.Contains("2x1") ||
                              lowerName.Contains("descuento");

        if (!isOffer && !hasPromoKeyword)
            return false;

        var match = Regex.Match(name, @"(\d+)\s*OFF", RegexOptions.IgnoreCase);
        if (match.Success && double.TryParse(match.Groups[1].Value, out var parsedPct))
        {
            discountPercentage = parsedPct;
        }
        else if (lowerName.Contains("3x2"))
        {
            discountPercentage = 33.3;
        }
        else if (lowerName.Contains("2x1"))
        {
            discountPercentage = 50.0;
        }

        return true;
    }

    private static string DetermineCategory(string name)
    {
        var lower = name.ToLowerInvariant();
        if (lower.Contains("pollo") || lower.Contains("nugget") || lower.Contains("mcnuggets"))
            return "chicken";
        if (lower.Contains("café") || lower.Contains("cafe") || lower.Contains("tostado") || lower.Contains("factura") || lower.Contains("criollo"))
            return "cafe";
        if (lower.Contains("mcflurry") || lower.Contains("sundae") || lower.Contains("cono") || lower.Contains("shake") || lower.Contains("helado"))
            return "dessert";
        if (lower.Contains("ensalada") || lower.Contains("veggie"))
            return "veggie";

        return "burger_beef";
    }
}
