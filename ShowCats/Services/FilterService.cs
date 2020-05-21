using ShowCats.Models;
using System.Linq;
using System.Collections.Generic;
using ShowCats.Models.Enum;
using ShowPets.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ShowCats.Services
{
    public class FilterService : IFilterService
    {
        public IDataService _dataService;
        public FilterService(IDataService dataService)
        {
            this._dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        /// <summary> 
        /// Filter Owner List with Pet type
        /// </summary>
        /// <returns>Dictionary value</returns>
        public async Task<Dictionary<string, List<string>>> FilterPets()
        {
            var response = await _dataService.GetDataAsync();

            var dict = new Dictionary<string, List<string>>();

            if (!string.IsNullOrEmpty(response.Error))
                dict.Add("Error", new List<string> { response.Error });

            else if (response.OwnerList.Count == 0)
                dict.Add("Error", new List<string> { "No Data to Process" });
            else
            {
                //Filter owners who have at least one cat
                var ownerWithPets = response.OwnerList.FindAll(o => o.Pets != null && o.Pets.Any(p => p.Type == PetType.Cat));

                if (ownerWithPets.Any())
                {
                    //Filter out all other type of pets, except cat 
                    ownerWithPets.ForEach(owner =>
                    {
                        owner.Pets = owner.Pets.FindAll(p => p.Type == PetType.Cat).ToList();
                    });

                    dict = MapGenderToPet(ownerWithPets);
                }
                else { dict.Add("Error", new List<string> { "No Data to Process" }); }
            }
            return dict;
        }

        /// <summary> 
        /// Map pets with owner's gender
        /// </summary>
        /// <param name="owners">List Of Owner with pets</param>
        /// <returns>Dictionary value</returns>
        private static Dictionary<string, List<string>> MapGenderToPet(List<Owner> ownerWithPets)
        {
            var dict = new Dictionary<string, List<string>>
            {
                [Gender.Male.ToString()] = ownerWithPets.FindAll(owner => Gender.Male == owner.Gender)
                                                .Select(owner => owner.Pets.Select(pet => pet.Name).ToList())
                                                .SelectMany(petName => petName).ToList(),

                [Gender.Female.ToString()] = ownerWithPets.FindAll(owner => Gender.Female == owner.Gender)
                                                .Select(owner => owner.Pets.Select(pet => pet.Name).ToList())
                                                .SelectMany(petName => petName).ToList()
            };

            return dict;
        }
    }
}
