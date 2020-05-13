using ShowCats.Models;
using ShowCats.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowPets.Services.Interfaces
{
    public interface IFilterService
    {
        Dictionary<Gender, List<string>> FilterPets(List<Owner> owners, List<PetType> typeList);
    }
}
