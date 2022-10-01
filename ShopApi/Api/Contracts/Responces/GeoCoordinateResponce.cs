namespace Api.Contracts.Responces
{
    public class GeoCoordinateResponce
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<ShopResponce> Shops { get; set; } 
    }
}
