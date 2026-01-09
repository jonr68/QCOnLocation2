using QcOnLocation.Models;
using QcOnLocation.Repository;

namespace QcOnLocation.Services;

public class LocationService
{
    private readonly LocationRepository _locationRepository = new();

    public IEnumerable<Location> GetLocations()
    {
        return _locationRepository.GetLocations();
    }

    public Location? GetLocationById(int id)
    {
        return _locationRepository.GetLocationById(id);
    }

    public Location CreateLocation(Location location)
    {
        return _locationRepository.CreateLocation(location);
    }

    public Location? DeleteLocation(int id)
    {
        return MockLocationData.DeleteLocation(id);
    }

    public Location? UpdateLocation(Location location)
    {
        return MockLocationData.UpdateLocation(location);
    }
}