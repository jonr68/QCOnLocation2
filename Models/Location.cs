using System.ComponentModel.DataAnnotations.Schema;

namespace QcOnLocation.Models
{
    public class Location
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string LatLong { get; set; }
        public string[]? Tags { get; set; }
        public string[]? Images { get; set; }
    }
}