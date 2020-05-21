using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShowCats.Models;
using ShowCats.Models.Enum;
using ShowCats.Services;
using ShowPets.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowPetsTest
{
    [TestClass]
    public class FilterServiceTests
    {
        private readonly Mock<IDataService> mockDataService;
        private readonly IFilterService filterService;

        public FilterServiceTests()
        {
            mockDataService = new Mock<IDataService>();
            filterService = new FilterService(mockDataService.Object);
        }

        [TestMethod]
        public async Task FilterPets_ShouldReturnList_ValidInputAsync()
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

            var response = new Response
            {
                Error = string.Empty,
                OwnerList = validOwnerList
            };

            mockDataService.Setup(m => m.GetDataAsync()).Returns(Task.FromResult(response));

            //Act
            var result = await filterService.FilterPets();

            //Assert
            Assert.IsInstanceOfType(result, typeof(Dictionary<string, List<string>>));
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.ContainsKey(Gender.Female.ToString()));
            Assert.IsTrue(result.ContainsKey(Gender.Male.ToString()));
        }

        [TestMethod]
        public async Task FilterPets_ShouldReturnNull_EmptyOwnerInputAsync()
        {
            //Arrange
            var validOwnerList = new List<Owner>();
            var response = new Response
            {
                Error = string.Empty,
                OwnerList = validOwnerList
            };

            mockDataService.Setup(m => m.GetDataAsync()).Returns(Task.FromResult(response));


            //Act
            var result = await filterService.FilterPets();

            //Assert
            Assert.IsTrue(result.ContainsKey("Error"));
        }

        [TestMethod]
        public async Task FilterPets_ShouldReturnNull_EmptyPetInputAsync()
        {
            //Arrange
            var validOwnerList = new List<Owner> {
                new Owner{ Name = "Jennifer",
                           Gender = Gender.Female,
                           Age = "18",
                           Pets = null
                }
            };

            var response = new Response
            {
                Error = string.Empty,
                OwnerList = validOwnerList
            };

            mockDataService.Setup(m => m.GetDataAsync()).Returns(Task.FromResult(response));

            //Act
            var result = await filterService.FilterPets();

            //Assert
            Assert.IsTrue(result.ContainsKey("Error"));
        }
    }
}
