using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            // Configure logger and add to services
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            // Get the "Appsettings" from our config and provide it to anyone who needs it.
            services.Configure<Appsettings>(options => Configuration.GetSection("PeopleAPIInfo").Bind(options));
            services.AddSingleton(sp => sp.GetService<IOptionsSnapshot<Appsettings>>().Value);

            services.AddHttpClient<IDataService, DataService>();
            services.AddTransient<IFilterService, FilterService>();

            // Add console for input and output for our App
            services.AddSingleton(Console.Out);
            services.AddSingleton(Console.In);

            // Add app
            services.AddTransient<App>();
        }
    }
}
