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
    public ActionResult<Location> GetLocationById(int id)
    {
        var location = _locationService.GetLocationById(id);
        if (location == null)
        {
            return NotFound();
        }

        return location;
    }

    [HttpPost]
    [Route("")]
    public ActionResult<Location> CreateLocation([FromBody] Location location)
    {
        var createdLocation = _locationService.CreateLocation(location);
        return CreatedAtAction(nameof(GetLocationById), new { id = createdLocation.Id }, createdLocation);
    }
    
    [HttpDelete]
    [Route("{id}")]
    
    public ActionResult<Location> DeleteLocation(int id)
    {
        var deletedLocation = _locationService.DeleteLocation(id);
        return deletedLocation;
    }
}