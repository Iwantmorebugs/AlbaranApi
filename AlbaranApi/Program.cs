using System.IO;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AlbaranApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host =
                Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureServices(services => services.AddAutofac())
                    .ConfigureWebHostDefaults(
                        webHostBuilder =>
                        {
                            webHostBuilder
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<Startup>();
                        })
                    .Build();

            await host.RunAsync();
        }
    }
}