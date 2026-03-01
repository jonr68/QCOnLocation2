using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QcOnLocation.Data;
using QcOnLocation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace QcOnLocation.Controllers;

[ApiController]
[Route("location")]
public class LocationController : ControllerBase
{
    private readonly LocationContext _context;
    private readonly IWebHostEnvironment _env;

    public LocationController(LocationContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> Get(int id)
    {
        var post = await _context.Locations.FindAsync(id);
        return post == null ? NotFound() : post;
    }

    [HttpPost]
    // Accept form-data for fields and files. Client should POST multipart/form-data with text fields and file inputs named 'images'.
    public async Task<ActionResult<Location>> Create([FromForm] Location location, [FromForm] IFormFile[]? images)
    {
        if (images != null && images.Length > 0)
        {
            var saved = await SaveImagesAsync(images);
            location.Images = saved.ToArray();
        }

        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
    }

    [HttpPut("{id}")]
    // Accept form-data for fields and files. To add images on update, include files named 'images'.
    public async Task<IActionResult> Update(int id, [FromForm] Location location, [FromForm] IFormFile[]? images)
    {
        if (id != location.Id) return BadRequest();

        var existing = await _context.Locations.FindAsync(id);
        if (existing == null) return NotFound();

        // Update scalar properties
        existing.Name = location.Name;
        existing.Description = location.Description;
        existing.LatLong = location.LatLong;
        existing.Tags = location.Tags;

        // If new images were uploaded, save and append them to existing images
        if (images != null && images.Length > 0)
        {
            var saved = await SaveImagesAsync(images);
            var current = existing.Images ?? Array.Empty<string>();
            existing.Images = current.Concat(saved).ToArray();
        }

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

    private async Task<List<string>> SaveImagesAsync(IFormFile[] images)
    {
        var uploadsRoot = Path.Combine(_env.ContentRootPath, "Images");
        Directory.CreateDirectory(uploadsRoot);

        var savedPaths = new List<string>();

        foreach (var file in images)
        {
            if (file == null || file.Length == 0) continue;

            // Basic content-type check - allow only images
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                // skip non-images (alternatively, you could reject the request)
                continue;
            }

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            savedPaths.Add(Path.Combine("Images", fileName).Replace("\\", "/"));
        }

        return savedPaths;
    }
}