using FastFoodDeals.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeals.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Deals.AnyAsync())
        {
            return; // Ya hay datos
        }

        var sampleDeals = new List<Deal>
        {
            new Deal
            {
                Id = Guid.NewGuid(),
                ExternalId = "mcd_cuarto_libra_01",
                Brand = "McDonald's",
                Title = "Combo Cuarto de Libra con Queso",
                Description = "Incluye papas medianas y gaseosa mediana a elección. Válido en locales de Córdoba.",
                OriginalPrice = 6500.00m,
                DiscountPrice = 4200.00m,
                DiscountPercentage = 35.4,
                Category = "burger_beef",
                CouponCode = "MCDCUARTO",
                RedemptionUrl = "https://www.mcdonalds.com.ar/promociones",
                ImageUrl = "https://cache-backend-mcd.mcdonaldscupones.com/media/image/product$kzXh3r9V/200/200/crop?gl=ar",
                LocationZone = "cordoba_all",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                LastScrapedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Deal
            {
                Id = Guid.NewGuid(),
                ExternalId = "bk_whopper_doble_01",
                Brand = "Burger King",
                Title = "Combo Whopper Doble con Queso",
                Description = "Doble carne a la parrilla, queso cheddar, papas medianas y bebida libre. Sucursales Córdoba.",
                OriginalPrice = 7800.00m,
                DiscountPrice = 4800.00m,
                DiscountPercentage = 38.5,
                Category = "burger_beef",
                CouponCode = "BKWHOPPER",
                RedemptionUrl = "https://www.burgerking.com.ar/promociones",
                ImageUrl = "https://bk-latam-prod.s3.amazonaws.com/sites/burgerking.com.ar/files/WhopperDobleConQueso_0.png",
                LocationZone = "cordoba_all",
                ExpiresAt = DateTime.UtcNow.AddDays(5),
                LastScrapedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Deal
            {
                Id = Guid.NewGuid(),
                ExternalId = "mcd_mcnuggets_10_01",
                Brand = "McDonald's",
                Title = "Combo McNuggets 10 Piezas",
                Description = "10 deliciosos McNuggets de pollo crujiente con 2 salsas a elección + papas y bebida.",
                OriginalPrice = 5400.00m,
                DiscountPrice = 3600.00m,
                DiscountPercentage = 33.3,
                Category = "chicken",
                CouponCode = "MCDNUGGETS10",
                RedemptionUrl = "https://www.mcdonalds.com.ar/promociones",
                ImageUrl = "https://cache-backend-mcd.mcdonaldscupones.com/media/image/product$kzXq9r1A/200/200/crop?gl=ar",
                LocationZone = "cordoba_all",
                ExpiresAt = DateTime.UtcNow.AddDays(6),
                LastScrapedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Deal
            {
                Id = Guid.NewGuid(),
                ExternalId = "bk_stacker_triple_01",
                Brand = "Burger King",
                Title = "Combo Stacker Triple XL",
                Description = "Tres carnes a la parrilla, salsa Stacker exclusiva, panceta crocante y cheddar + papas y gaseosa.",
                OriginalPrice = 8900.00m,
                DiscountPrice = 5200.00m,
                DiscountPercentage = 41.6,
                Category = "burger_beef",
                CouponCode = "BKSTACKER3",
                RedemptionUrl = "https://www.burgerking.com.ar/promociones",
                ImageUrl = "https://bk-latam-prod.s3.amazonaws.com/sites/burgerking.com.ar/files/StackerTriple_0.png",
                LocationZone = "cordoba_all",
                ExpiresAt = DateTime.UtcNow.AddDays(4),
                LastScrapedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        await context.Deals.AddRangeAsync(sampleDeals);
        await context.SaveChangesAsync();
    }
}
