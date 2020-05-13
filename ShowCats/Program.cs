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
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //App started: Get data to process
            var dataServ = serviceProvider.GetService<IDataService>();
            var response = await dataServ.GetData();

            if (!string.IsNullOrEmpty(response.Error))
            {
                Console.WriteLine(response.Error);
            }
            else if (response.OwnerList.Count == 0)
            {
                Console.WriteLine("No Data to Process");
            }
            else
            {
                //If get data is success then process data
                var filterServ = serviceProvider.GetService<IFilterService>();
                var result = filterServ.FilterPets(response.OwnerList, new List<PetType> { PetType.Cat });

                if (result != null)
                {
                    foreach (KeyValuePair<Gender, List<string>> entry in result)
                    {
                        Console.WriteLine(entry.Key);
                        Console.WriteLine();
                        entry.Value.ForEach(petName => Console.WriteLine($" {petName}"));
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Error in Data Processing");
                }
            }
            Console.ReadLine();
        }
    }
}
