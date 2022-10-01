using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Requests
{
    public class GeoCoordinateRequest
    {
        [Required]
        [Range(typeof(double), "-90", "90")]
        public double Latitude { get; set; }

        [Required]
        [Range(typeof(double), "-180", "180")]
        public double Longitude { get; set; }
    }
}
