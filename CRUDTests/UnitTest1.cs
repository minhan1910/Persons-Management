using CRUDTests.Factory;
using Entities;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Utils;
using Services.Helpers;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _outputHelper;
        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _outputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            var mm = new MyMath();
            int input1 = 10, input2 = 5;
            int expected = 15;

            // Act
            int actual = mm.Add(input1, input2);

            // Assert
            Assert.Equal(expected, actual); 
        }


        [Fact]
        public void PropertiesHandler_Copy_PersonAddReqeust_To_Person()
        {
            PersonAddRequest personAddRequest = A.PersonAddRequest;
            Person? person = PropertiesHandler<PersonAddRequest, Person>.Copy(personAddRequest, Person.Create());


            _outputHelper.WriteLine($"PersonAddRequest:");
            _outputHelper.WriteLine($"{personAddRequest.PersonName}, Gender: {personAddRequest.Gender}");
            _outputHelper.WriteLine($"Person:");
            _outputHelper.WriteLine($"{person?.PersonName}, Gender: {person?.Gender}");            

            Assert.True(personAddRequest.Gender.ToString() == person?.Gender);
        }
    }
}