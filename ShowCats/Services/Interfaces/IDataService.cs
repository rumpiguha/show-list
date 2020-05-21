using ShowCats.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowPets.Services.Interfaces
{
    public interface IDataService
    {
        Task<Response> GetDataAsync();
    }
}
