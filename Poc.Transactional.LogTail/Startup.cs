using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Poc.LogTail.Core.Configuration;
using Poc.LogTail.Core.DataAccess.Mongo;
using Poc.LogTail.Core.Repositories;
using Poc.LogTail.Core.Repositories.Contracts;
using Poc.LogTail.Core.Services;
using Poc.LogTail.Core.Services.Contracts;

namespace Poc.Transactional.LogTail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["Settings:MongoDbSettings:ConnectionString"];
            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));
            services.AddSingleton(x => x.GetRequiredService<IOptions<Settings>>().Value);

            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
            services.AddSingleton<IDbContext<IMongoDatabase>, MongoDbContext>();

            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IProductService, ProductService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Poc.Transactional.LogTail", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poc.Transactional.LogTail v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
