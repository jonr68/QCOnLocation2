using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QcOnLocation.Data;
using QcOnLocation.Models;


namespace QcOnLocation.Controllers;

/**
 * The Controller is responsible for endpoint routing.
 */
[ApiController]
[Route("locations")]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly LocationContext _context;

    public LocationsController(LocationContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Location>>> GetAll()
    {
        return await _context.Locations.ToListAsync();
    }
}