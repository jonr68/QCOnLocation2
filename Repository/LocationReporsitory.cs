using QcOnLocation.Models;

namespace QcOnLocation.Repository;

public class LocationRepository
{
    public IEnumerable<Location> GetLocations()
    {
        return MockLocationData.GetMockLocationData();
    }
}

public static class MockLocationData
{
    public static Location[] GetMockLocationData() =>
    [
        new()
        {
            Uuid = 1, Name = "Location A", Description = "Description A", LatLong = "34.0522 N-118.2437",
            Tags = "tag1,tag2"
        },
        new()
        {
            Uuid = 2, Name = "Location B", Description = "Description B", LatLong = "40.7128 N-74.0060",
            Tags = "tag3,tag4"
        },
        new()
        {
            Uuid = 3, Name = "Location C", Description = "Description C", LatLong = "37.7749 N-122.4194",
            Tags = "tag5,tag6"
        }
    ];
}