using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QcOnLocation.Data;
using QcOnLocation.Models;
using QcOnLocation.Services;

namespace QcOnLocation.Controllers;

/**
 * The Controller is responsible for endpoint routing.
 */
[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly LocationContext _context;

    public LocationsController(LocationContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetAll()
    {
        return await _context.Locations.ToListAsync();
    }
}