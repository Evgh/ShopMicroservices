namespace InfrastuctureLayer.Helpers
{
    internal static class GeoCoordinateHashHelper
    {
        internal static string CountGeoHash(double lattitude, double longtitude)
        {
            int lattitudeHash = lattitude.GetHashCode();
            int longtitudeHath = longtitude.GetHashCode();
            int sumHash = (lattitude + longtitude).GetHashCode();

            byte[] lattitudeBytes = BitConverter.GetBytes(lattitudeHash);
            byte[] longtitudeBytes = BitConverter.GetBytes(longtitudeHath);
            byte[] sumBytes = BitConverter.GetBytes(sumHash);

            byte[] allBytes = lattitudeBytes.Concat(longtitudeBytes).Concat(sumBytes).ToArray();

            return Convert.ToHexString(allBytes);
        }
    }
}
