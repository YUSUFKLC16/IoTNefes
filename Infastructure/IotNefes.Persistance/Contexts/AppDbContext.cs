using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace IotNefes.Persistance.Contexts
{
    public class MongoDbContext
    {
        private static bool _conventionsRegistered;
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client, string databaseName)
        {
            RegisterConventions();
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string? name = null)
        {
            return _database.GetCollection<T>(name ?? typeof(T).Name.ToLowerInvariant());
        }

        private static void RegisterConventions()
        {
            if (_conventionsRegistered) return;
            _conventionsRegistered = true;

            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("IotNefesConventions", pack, _ => true);

            BsonSerializer.RegisterIdGenerator(typeof(string), StringObjectIdGenerator.Instance);
            BsonSerializer.RegisterSerializer(new ObjectSerializer(ObjectSerializer.AllAllowedTypes));
        }
    }
}
