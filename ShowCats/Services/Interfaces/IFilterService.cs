using ShowCats.Models;
using ShowCats.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowPets.Services.Interfaces
{
    public interface IFilterService
    {
        Task<Dictionary<string, List<string>>> FilterPets();
    }
}
