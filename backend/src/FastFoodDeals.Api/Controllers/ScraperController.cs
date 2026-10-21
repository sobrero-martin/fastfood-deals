using FastFoodDeals.Infrastructure.Scrapers;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodDeals.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScraperController : ControllerBase
{
    private readonly McDonaldsScraper _mcdonaldsScraper;
    private readonly ILogger<ScraperController> _logger;

    public ScraperController(McDonaldsScraper mcdonaldsScraper, ILogger<ScraperController> logger)
    {
        _mcdonaldsScraper = mcdonaldsScraper;
        _logger = logger;
    }

    /// <summary>
    /// POST /api/scraper/mcdonalds
    /// Dispara manualmente la sincronización de ofertas de McDonald's Argentina.
    /// </summary>
    [HttpPost("mcdonalds")]
    [HttpGet("mcdonalds")] // Permite dispararlo también abriendo la URL en el navegador
    public async Task<IActionResult> RunMcDonaldsScraper()
    {
        try
        {
            var count = await _mcdonaldsScraper.ScrapeAndSyncAsync();
            return Ok(new
            {
                status = "success",
                brand = "McDonald's",
                dealsProcessed = count,
                executedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar el scraper de McDonald's");
            return StatusCode(500, new
            {
                status = "error",
                message = ex.Message
            });
        }
    }
}
