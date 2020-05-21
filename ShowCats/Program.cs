using Microsoft.Extensions.DependencyInjection;

using ShowCats.Models.Enum;
using ShowPets;
using ShowPets.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowCats
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build our service provider
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Run our app
            serviceProvider.GetService<App>().RunAsync().Wait();
        }
    }
}
