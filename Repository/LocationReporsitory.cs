using System;
using System.Collections.Generic;
using System.Linq;
using QcOnLocation.Models;

namespace QcOnLocation.Repository;

public class LocationRepository
{
    public IEnumerable<Location> GetLocations()
    {
        return MockLocationData.GetMockLocationData();
    }

    public Location? GetLocationById(int id)
    {
        return MockLocationData.GetMockLocationData().FirstOrDefault(l => l.Id == id);
    }

    public Location CreateLocation(Location location)
    {
        return MockLocationData.AddLocation(location);
    }
}

public static class MockLocationData
{
    private static readonly List<Location> Data = new()
    {
        new Location
        {
            Id = 1, Name = "Location A", Description = "Description A", LatLong = "34.0522 N-118.2437",
            Tags = new[] { "tag1", "tag2" }
        },
        new Location
        {
            Id = 2, Name = "Location B", Description = "Description B", LatLong = "40.7128 N-74.0060",
            Tags = new[] { "tag3", "tag4" }
        },
        new Location
        {
            Id = 3, Name = "Location C", Description = "Description C", LatLong = "37.7749 N-122.4194",
            Tags = new[] { "tag5", "tag6" }
        }
    };

    public static IEnumerable<Location> GetMockLocationData() => Data;

    public static Location AddLocation(Location location)
    {
        if (location is null) throw new ArgumentNullException(nameof(location));

        if (!location.Id.HasValue || location.Id == 0)
        {
            var nextId = Data.Any() ? Data.Max(l => l.Id ?? 0) + 1 : 1;
            location.Id = nextId;
        }

        Data.Add(location);
        return location;
    }
    
    public static Location DeleteLocation(int id)
    {
        var location = Data.FirstOrDefault(l => l.Id == id);
        if (location != null)
        {
            Data.Remove(location);
        }
        else
        {
            throw new ArgumentNullException(nameof(location)); 
        }
        return location;
    }
}