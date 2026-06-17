namespace IotNefes.Abstractions.AirTemperature.Dto
{
    public class AirTemperatureDto
    {
        public string Id { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}