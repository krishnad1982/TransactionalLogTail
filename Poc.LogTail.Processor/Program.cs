using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Poc.LogTail.Core.Configuration;
using Poc.LogTail.Core.Contracts;
using Poc.LogTail.Core.DataAccess.Mongo;
using Poc.LogTail.Core.Repositories;
using Poc.LogTail.Core.Repositories.Contracts;
using Poc.LogTail.Core.Services;
using Poc.LogTail.Core.Services.Contracts;


namespace Poc.LogTail.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            var connectionString = configuration["Settings:MongoDbSettings:ConnectionString"];

            //services.AddSingleton(configuration);
            services.Configure<Settings>(configuration.GetSection(nameof(Settings)));
            services.AddSingleton(x => x.GetRequiredService<IOptions<Settings>>().Value);

            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
            services.AddSingleton<IDbContext<IMongoDatabase>, MongoDbContext>();
            services.AddSingleton<IStreamFeed, StreamFeed>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IProductService, ProductService>();
            var serviceProvider = services.BuildServiceProvider();


            var service = serviceProvider.GetService<IStreamFeed>();
            service?.StartFeed().GetAwaiter().GetResult();
        }
    }
}
