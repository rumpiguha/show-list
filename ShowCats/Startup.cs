using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShowCats.Services;
using ShowPets.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShowPets
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var configBuilder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json");

            Configuration = configBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddHttpClient<IDataService, DataService>();
            services.AddSingleton<IFilterService, FilterService>();
        }
    }
}
