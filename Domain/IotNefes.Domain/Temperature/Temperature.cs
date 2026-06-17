namespace IotNefes.Domain.Temperature
{
    public class Temperature : BaseEntity
    {
        public string DeviceId { get; set; } = string.Empty;
        public double Value { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
