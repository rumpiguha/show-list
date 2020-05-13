using ShowCats.Models;
using System.Linq;
using System.Collections.Generic;
using ShowCats.Models.Enum;
using ShowPets.Services.Interfaces;

namespace ShowCats.Services
{
    public class FilterService : IFilterService
    {
        /// <summary> 
        /// Filter Owner List with Pet type
        /// </summary>
        /// <param name="owners">List Of Owner to filter</param>
        /// <param name="typeList">List of pet type to be filtered with</param>
        /// <returns>int value</returns>
        public Dictionary<Gender, List<string>> FilterPets(List<Owner> owners, List<PetType> typeList)
        {
            if (owners.Any() && typeList.Any())
            {
                //Filter owners who have at least one pet with input pet type  
                var ownerWithPets = owners.FindAll(o => o.Pets != null && o.Pets.Any(p => p.Type == typeList.FirstOrDefault()));

                if (ownerWithPets.Any())
                {
                    //Filter out all other type of pets, except input pet type  
                    ownerWithPets.ForEach(owner => { owner.Pets = owner.Pets.FindAll(p => p.Type == typeList.FirstOrDefault()).ToList(); });

                    return MapGenderToPet(ownerWithPets);
                }
            }
            return null;
        }

        /// <summary> 
        /// Map pets with owner's gender
        /// </summary>
        /// <param name="owners">List Of Owner with pets</param>
        /// <returns>Dictionary value</returns>
        private static Dictionary<Gender, List<string>> MapGenderToPet(List<Owner> ownerWithPets)
        {
            var dict = new Dictionary<Gender, List<string>>
            {
                [Gender.Male] = ownerWithPets.FindAll(owner => Gender.Male == owner.Gender)
                                                .Select(owner => owner.Pets.Select(pet => pet.Name).ToList())
                                                .SelectMany(petName => petName).ToList(),

                [Gender.Female] = ownerWithPets.FindAll(owner => Gender.Female == owner.Gender)
                                                .Select(owner => owner.Pets.Select(pet => pet.Name).ToList())
                                                .SelectMany(petName => petName).ToList()
            };

            return dict;

        }
    }
}
