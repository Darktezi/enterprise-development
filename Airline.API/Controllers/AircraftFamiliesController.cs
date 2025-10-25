using Airline.Infrastructure.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airline.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AircraftFamiliesController : ControllerBase
{
    private readonly AirlineDbContext _context;

    public AircraftFamiliesController(AirlineDbContext context)
    {
        _context = context;
    }

    // GET: api/AircraftFamilies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetFamilies()
    {
        var families = await _context.Families
            .Select(f => new
            {
                f.Id,
                f.Name,
                f.Manufacturer,
                ModelsCount = f.Models.Count
            })
            .ToListAsync();

        return Ok(families);
    }
}