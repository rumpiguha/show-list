using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShowCats.Models;
using ShowPets.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShowCats.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public DataService(HttpClient httpClient, IConfigurationRoot config)
        {
            var baseUrl = config["dataUrl"];
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }
        public async Task<Response> GetData()
        {
            var result = new Response
            {
                OwnerList = new List<Owner>(),
                Error = string.Empty
            };

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();

                result.OwnerList = JsonConvert.DeserializeObject<List<Owner>>(data.Result);
            }
            else
            {
                result.Error = $"Service not reachable:error code {response.StatusCode}";
            }

            return result;
        }
    }
}
