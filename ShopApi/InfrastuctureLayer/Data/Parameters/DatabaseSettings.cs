namespace InfrastuctureLayer.Data.Parameters
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ShopsCollectionName { get; set; } = null!;
        public string LocationsCollectionName { get; set; } = null!;
    }
}
