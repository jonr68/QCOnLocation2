using Microsoft.AspNetCore.Mvc;
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
    private readonly LocationService _locationService = new();


    [HttpGet]
    [Route("")]
    public IEnumerable<Location> Get()
    {
        return _locationService.GetLocations();
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Location> GetLocationById(Guid id)
    {
        var location = _locationService.GetLocationById(id);
        if (location == null)
        {
            return NotFound();
        }
        return location;
    }
}

