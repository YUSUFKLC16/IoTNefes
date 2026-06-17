using System.ComponentModel.DataAnnotations;

namespace IotNefes.WebApi.src.Models
{
    public class Co2Request
    {
        [Required]
        public string DeviceId { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000)]
        public double Value { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }
    }
}
