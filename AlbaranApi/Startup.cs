using AlbaranApi.Contracts;
using AlbaranApi.Models.Context;
using Autofac;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AlbaranApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            var environmentName = webHostEnvironment.EnvironmentName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(webHostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("SqlConnection");

            services.AddControllers();

            services.AddScoped<IEntradaContext, EntradaContext>();


            services.AddDbContext<EntradaContext>(o => o.UseSqlServer(connectionString));

            services.AddSwaggerGen(swagger =>
            {
                var contact =
                    new OpenApiContact
                    {
                        Name = SwaggerConfiguration.ContactName
                    };

                var info =
                    new OpenApiInfo
                    {
                        Title = SwaggerConfiguration.DocInfoTitle,
                        Version = SwaggerConfiguration.DocInfoVersion,
                        Description = SwaggerConfiguration.DocInfoDescription,
                        Contact = contact
                    };

                swagger.SwaggerDoc(SwaggerConfiguration.DocNameV1, info);
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            var hostAssembly = typeof(Startup).Assembly;
            containerBuilder.RegisterAssemblyModules(hostAssembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBusControl bus)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Development")) app.UseDeveloperExceptionPage();

            // app.UseHttpsRedirection();
            app.UseHsts();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointDescription);
            });

            TaskUtil.Await(() => bus.StartAsync());
        }
    }
}