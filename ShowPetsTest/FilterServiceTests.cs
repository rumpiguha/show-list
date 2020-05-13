using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShowCats.Models;
using ShowCats.Models.Enum;
using ShowCats.Services;
using System.Collections.Generic;

namespace ShowPetsTest
{
    [TestClass]
    public class FilterServiceTests
    {
        [TestMethod]
        public void FilterPets_ShouldReturnList_ValidInput()
        {
            //Arrange
            var validOwnerList = new List<Owner> {
                new Owner{ Name = "Jennifer",
                           Gender = Gender.Female,
                           Age = "18",
                           Pets = new List<Pet>{
                              new Pet { Name = "Garfield", Type = PetType.Cat}
                           }
                },
                new Owner{ Name = "Steve",
                           Gender = Gender.Male,
                           Age = "18",
                           Pets = new List<Pet>{
                              new Pet { Name = "Tom", Type = PetType.Cat},
                              new Pet { Name = "Sam", Type = PetType.Dog}
                           }
                }
            };

            var validTypeList = new List<PetType> { PetType.Cat };

            //Act
            var filterService = new FilterService();
            var result = filterService.FilterPets(validOwnerList, validTypeList);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Dictionary<Gender, List<string>>));
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.ContainsKey(validOwnerList[0].Gender));
        }

        [TestMethod]
        public void FilterPets_ShouldReturnNull_EmptyOwnerInput()
        {
            //Arrange
            var validOwnerList = new List<Owner>();

            var validTypeList = new List<PetType> { PetType.Cat };

            //Act
            var filterService = new FilterService();
            var result = filterService.FilterPets(validOwnerList, validTypeList);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FilterPets_ShouldReturnNull_EmptyPetInput()
        {
            //Arrange
            var validOwnerList = new List<Owner> {
                new Owner{ Name = "Jennifer",
                           Gender = Gender.Female,
                           Age = "18",
                           Pets = null
                }
            };

            var validTypeList = new List<PetType> { PetType.Cat };

            //Act
            var filterService = new FilterService();
            var result = filterService.FilterPets(validOwnerList, validTypeList);

            //Assert
            Assert.IsNull(result);
        }
    }
}
