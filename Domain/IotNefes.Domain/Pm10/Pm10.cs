namespace IotNefes.Domain.Pm10
{
    public class Pm10 : BaseEntity
    {
        public string DeviceId { get; set; } = string.Empty;
        public double Value { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
