namespace Poc.LogTail.Core.Configuration
{
    public class Settings
    {
        public MongoDbSettings MongoDbSettings { get; set; }
    }

    public class MongoDbSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
