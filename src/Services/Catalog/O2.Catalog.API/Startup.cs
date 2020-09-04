using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using O2.Catalog.API.Data;

namespace O2.Catalog.API
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
            services.Configure<CatalogSettings>(Configuration);


            //string connectionString =

            var server = Configuration["DatabaseServer"];
            var database = Configuration["DatabaseName"];
            var user = Configuration["DatabaseUser"];
            var password = Configuration["DatabaseUserPassword"];
            var connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", server, database, user, password);
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));


            services.AddMvc();

            services.AddSwaggerGen(option =>
            {
                option.DescribeAllEnumsAsStrings();
                option.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                      Title="O2 Catalog API - Product Catalog HTTP API",
                      Version="v1",
                      Description="The Product Catalog Microservice HTTP API. This is a Data-Drive/CRUID microservice",
                      TermsOfService="Termns of Service"
                });
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c=>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "O2 Catalog API V1");
            });
            app.UseMvc();
        }
    }
}
