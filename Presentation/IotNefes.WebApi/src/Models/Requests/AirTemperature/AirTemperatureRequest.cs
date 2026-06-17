using System.ComponentModel.DataAnnotations;

namespace IotNefes.WebApi.src.Models
{
    public class AirTemperatureRequest
    {
        [Required]
        public string DeviceId { get; set; } = string.Empty;

        [Required]
        [Range(-60, 60)]
        public double Temperature { get; set; }

        [Range(0, 100)]
        public double? Humidity { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }
    }
}
