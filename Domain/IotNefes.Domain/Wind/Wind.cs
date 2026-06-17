namespace IotNefes.Domain.Wind
{
    public class Wind : BaseEntity
    {
        public string DeviceId { get; set; } = string.Empty;
        public double Speed { get; set; }
        public double Direction { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
