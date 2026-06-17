namespace IotNefes.Domain.AirTemperature
{
    public class AirTemperature : BaseEntity
    {
        public string DeviceId { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
