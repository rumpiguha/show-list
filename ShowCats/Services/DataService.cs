using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShowCats.Models;
using ShowPets;
using ShowPets.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShowCats.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DataService> logger;
        private readonly Appsettings appsettings;
        public const string errorMsg = "Service not reachable:error code ";
        public DataService(ILogger<DataService> logger,
            Appsettings appsettings,
            HttpClient httpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.appsettings = appsettings ?? throw new ArgumentNullException(nameof(appsettings));
            this._httpClient = httpClient;
        }

        /// <summary>
        /// Gets the pet owner list from the people service
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetDataAsync()
        {
            var result = new Response
            {
                OwnerList = new List<Owner>(),
                Error = string.Empty
            };

            HttpResponseMessage response = await _httpClient.GetAsync(this.appsettings.EndPoint);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();

                result.OwnerList = JsonConvert.DeserializeObject<List<Owner>>(data.Result);
            }
            else
            {
                result.Error = $"{errorMsg}{response.StatusCode}";
            }

            return result;
        }
    }
}
