namespace MyPasswordManager.Infrastructure.CosmosDb
{
    public class CosmosDbConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? ContainerName { get; set; }
    }
}
