namespace DomainLayer.Entities
{
    public class GeoCoordinateEntity
    {
        private const byte _maxLatitude = 90;
        private const byte _maxLongtitude = 180;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<ShopEntity> Shops { get; set; }

        public bool IsValid()
        {
            return Math.Abs(Latitude) <= _maxLatitude &&
                   Math.Abs(Longitude) <= _maxLongtitude;
        }
    }
}
