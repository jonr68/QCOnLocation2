using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QcOnLocation.Controllers;
using QcOnLocation.Data;
using QcOnLocation.Models;
using Xunit;

namespace QcOnLocation.Controllers;

public class LocationsControllerTest
{
    private LocationContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<LocationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new LocationContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task GetAll_ReturnsAllLocations()
    {
        // Arrange
        var context = GetDatabaseContext();
        context.Locations.Add(new Location { Id = 1, Name = "Location 1", LatLong = "0,0", Description = "Description 1" });
        context.Locations.Add(new Location { Id = 2, Name = "Location 2", LatLong = "1,1", Description = "Description 2" });
        await context.SaveChangesAsync();

        var controller = new LocationsController(context);

        // Act
        var result = await controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Location>>>(result);
        var locations = Assert.IsAssignableFrom<IEnumerable<Location>>(actionResult.Value);
        Assert.Equal(2, locations.Count());
    }
}
