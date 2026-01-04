using Microsoft.AspNetCore.Mvc;
using QcOnLocation.Models;
using QcOnLocation.Services;

namespace QcOnLocation.Controllers;

[ApiController]
[Route("location")]
public class LocationController : ControllerBase
{
    private readonly LocationService _locationService = new();

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