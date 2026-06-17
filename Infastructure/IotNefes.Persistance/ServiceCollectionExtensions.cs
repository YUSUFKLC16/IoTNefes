using IotNefes.Infastructure.Abstraction;
using IotNefes.Persistance.Contexts;
using IotNefes.Persistance.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace IotNefes.Persistance
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString")
                ?? "mongodb://localhost:27017";
            var databaseName = configuration.GetValue<string>("MongoDb:DatabaseName")
                ?? "IotNefesDb";

            services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return new MongoDbContext(client, databaseName);
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
