using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QcOnLocation.Data;
using QcOnLocation.Models;
// using QcOnLocation.Services;

namespace QcOnLocation.Controllers;

[ApiController]
[Route("location")]
public class LocationController : ControllerBase
{
    private readonly LocationContext _context;

    public LocationController(LocationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetAll()
    {
        return await _context.Locations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> Get(int id)
    {
        var post = await _context.Locations.FindAsync(id);
        return post == null ? NotFound() : post;
    }

    [HttpPost]
    public async Task<ActionResult<Location>> Create(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Location location)
    {
        if (id != location.Id) return BadRequest();
        _context.Entry(location).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Locations.FindAsync(id);
        if (post == null) return NotFound();
        _context.Locations.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
