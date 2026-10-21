using FastFoodDeals.Core.DTOs;
using FastFoodDeals.Core.Entities;
using FastFoodDeals.Core.Interfaces;
using FastFoodDeals.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeals.Infrastructure.Repositories;

public class DealRepository : IDealRepository
{
    private readonly AppDbContext _context;

    public DealRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DealDto>> GetActiveDealsAsync(DealQueryDto? query = null)
    {
        var q = _context.Deals.Where(d => d.IsActive).AsQueryable();

        if (query is not null)
        {
            if (!string.IsNullOrWhiteSpace(query.Brand))
                q = q.Where(d => d.Brand == query.Brand);

            if (!string.IsNullOrWhiteSpace(query.Category))
                q = q.Where(d => d.Category == query.Category);

            if (!string.IsNullOrWhiteSpace(query.LocationZone))
                q = q.Where(d => d.LocationZone == query.LocationZone || d.LocationZone == "cordoba_all");
        }

        return await q
            .OrderByDescending(d => d.DiscountPercentage)
            .Select(d => new DealDto(
                d.Id,
                d.Brand,
                d.Title,
                d.Description,
                d.OriginalPrice,
                d.DiscountPrice,
                d.DiscountPercentage,
                d.Category,
                d.CouponCode,
                d.RedemptionUrl,
                d.ImageUrl,
                d.LocationZone,
                d.ExpiresAt
            ))
            .ToListAsync();
    }

    public async Task<DealDto?> GetByIdAsync(Guid id)
    {
        var deal = await _context.Deals.FindAsync(id);
        if (deal is null || !deal.IsActive) return null;

        return new DealDto(
            deal.Id,
            deal.Brand,
            deal.Title,
            deal.Description,
            deal.OriginalPrice,
            deal.DiscountPrice,
            deal.DiscountPercentage,
            deal.Category,
            deal.CouponCode,
            deal.RedemptionUrl,
            deal.ImageUrl,
            deal.LocationZone,
            deal.ExpiresAt
        );
    }

    public async Task<bool> UpsertAsync(Deal deal)
    {
        var existing = await _context.Deals
            .FirstOrDefaultAsync(d => d.Brand == deal.Brand && d.ExternalId == deal.ExternalId);

        if (existing is null)
        {
            await _context.Deals.AddAsync(deal);
            await _context.SaveChangesAsync();
            return true; // insertado nuevo
        }

        // Actualizar campos mutables
        existing.Title = deal.Title;
        existing.Description = deal.Description;
        existing.OriginalPrice = deal.OriginalPrice;
        existing.DiscountPrice = deal.DiscountPrice;
        existing.DiscountPercentage = deal.DiscountPercentage;
        existing.Category = deal.Category;
        existing.CouponCode = deal.CouponCode;
        existing.RedemptionUrl = deal.RedemptionUrl;
        existing.ImageUrl = deal.ImageUrl;
        existing.LocationZone = deal.LocationZone;
        existing.ExpiresAt = deal.ExpiresAt;
        existing.LastScrapedAt = DateTime.UtcNow;
        existing.IsActive = true;

        await _context.SaveChangesAsync();
        return false; // actualizado
    }

    public async Task DeactivateStaleDealsAsync(string brand, IEnumerable<string> activeExternalIds)
    {
        var stale = await _context.Deals
            .Where(d => d.Brand == brand && d.IsActive && !activeExternalIds.Contains(d.ExternalId))
            .ToListAsync();

        foreach (var deal in stale)
            deal.IsActive = false;

        if (stale.Count > 0)
            await _context.SaveChangesAsync();
    }

    public async Task RecordClickAsync(Guid dealId)
    {
        var click = new DealClick
        {
            DealId = dealId,
            ClickedAtUtc = DateTime.UtcNow
        };

        await _context.DealClicks.AddAsync(click);
        await _context.SaveChangesAsync();
    }
}
