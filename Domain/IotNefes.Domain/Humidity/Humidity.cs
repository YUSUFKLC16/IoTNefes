namespace IotNefes.Domain.Humidity
{
    public class Humidity : BaseEntity
    {
        public string DeviceId { get; set; } = string.Empty;
        public double Value { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
