using FastFoodDeals.Core.DTOs;
using FastFoodDeals.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodDeals.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DealsController : ControllerBase
{
    private readonly IDealRepository _dealRepository;

    public DealsController(IDealRepository dealRepository)
    {
        _dealRepository = dealRepository;
    }

    /// <summary>
    /// GET /api/deals
    /// Devuelve los cupones activos. Soporta filtros opcionales por query string:
    ///   ?brand=McDonald's
    ///   ?category=burger_beef
    ///   ?locationZone=cordoba_centro
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DealDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeals(
        [FromQuery] string? brand = null,
        [FromQuery] string? category = null,
        [FromQuery] string? locationZone = null)
    {
        var query = new DealQueryDto(brand, category, locationZone);
        var deals = await _dealRepository.GetActiveDealsAsync(query);
        return Ok(deals);
    }

    /// <summary>
    /// GET /api/deals/{id}
    /// Devuelve un cupón específico por su ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDealById(Guid id)
    {
        var deal = await _dealRepository.GetByIdAsync(id);
        if (deal is null) return NotFound();
        return Ok(deal);
    }
}
