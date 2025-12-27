namespace QcOnLocation.Models
{
    public class Location
    {
        public required int Uuid { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string LatLong { get; set; }


        public string? Tags { get; set; }
    }
}