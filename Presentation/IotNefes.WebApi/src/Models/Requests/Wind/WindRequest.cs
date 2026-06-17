using System.ComponentModel.DataAnnotations;

namespace IotNefes.WebApi.src.Models
{
    public class WindRequest
    {
        [Required]
        public string DeviceId { get; set; } = string.Empty;

        [Required]
        [Range(0, 200)]
        public double Speed { get; set; }

        [Required]
        [Range(0, 360)]
        public double Direction { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }
    }
}
