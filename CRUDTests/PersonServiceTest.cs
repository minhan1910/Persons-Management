using ServiceContracts;
using ServiceContracts.DTO.PersonDTO;
using Services;
using ServiceContracts.Enums;
using ServiceContracts.DTO;
using Xunit.Abstractions;
using CRUDTests.Factory;
using CRUDTests.Factory.Country;
using Entities;
using Services.Helpers;
using System.Linq;
using Services.Extensions;

namespace CRUDTests
{    
    public class PersonServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService(false);
            _personService = new PersonService(_countriesService, false);
            _testOutputHelper = testOutputHelper;
        }

        #region Test Helper 

        public CountryResponse GetCountryResponseByAddingCountryAddRequest(CountryAddRequestBuilder countryAddRequestBuilder)
            =>  _countriesService.AddCountry(countryAddRequestBuilder.Build());

        public PersonResponse GetPersonResponseByAddPersonRequest(PersonAddRequestBuilder personAddRequestBuilder)
            => _personService.AddPerson(personAddRequestBuilder.Build());

        public PersonUpdateRequest CreatePersonUpdateRequest(CountryAddRequestBuilder? countryAddRequestBuilder = null,
                                                             PersonAddRequestBuilder? personAddRequestBuilder = null)
        {
            // Arrange            
            CountryResponse countryResponse = GetCountryResponseByAddingCountryAddRequest(countryAddRequestBuilder ?? A.CountryAddRequest);
            // make sure countryId from PersonAddRequestBuilder as default
            Guid countryId = IsGuidNullOrEmpty(countryResponse.CountryID) ? PersonAddRequestBuilder.TEST_PERSON_ADD_REQUEST_COUNTRY_ID : countryResponse.CountryID;

            var personAddRequest = A.PersonAddRequest
                                    .WithCountryId(countryId);                                    

            //_testOutputHelper.WriteLine("Person add request");
            //_testOutputHelper.WriteLine(personAddRequest.Gender.ToString());            

            PersonResponse personResponse = GetPersonResponseByAddPersonRequest(personAddRequestBuilder ?? personAddRequest);

            //_testOutputHelper.WriteLine("Person response before converting to peron update request");
            //_testOutputHelper.WriteLine(personResponse.ToString());

            // set person name to test
            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();

            return personUpdateRequest;
        }

        public bool IsGuidNullOrEmpty(Guid? guid)
        {
            return !guid.HasValue || guid.Value == Guid.Empty;
        }

        public List<PersonAddRequest> CreatePersonAddRequestList(CountryResponse countryResponse)
        {
            Guid countryId = IsGuidNullOrEmpty(countryResponse.CountryID) ? PersonAddRequestBuilder.TEST_PERSON_ADD_REQUEST_COUNTRY_ID : countryResponse.CountryID;
            return new()
            {
                A.PersonAddRequest
                 .WithName("An")
                 .WithEmail("An@gmail.com")
                 .WithCountryId(countryId),
                A.PersonAddRequest
                 .WithName("an")
                 .WithEmail("An@gmail.com")
                 .WithCountryId(countryId),
                A.PersonAddRequest
                 .WithName("Minan")
                 .WithEmail("An1@gmail.com")
                 .WithCountryId(countryId),
                A.PersonAddRequest
                 .WithName("Hun")
                 .WithEmail("Hun@gmail.com")
                 .WithCountryId(countryId),
            };
        }
    #endregion

        #region AddPerson

    // When we supply null values as PersonAddRequest, it should throw ArgumentNullException
    [Fact]
        public void AddPerson_WithNullPersonAddRequest_ShouldThrowArgumentNullException()
        {
            PersonAddRequest? personAddRequest = null;

            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
        }

        // When we supply PersonName value is Null as PersonAddRequest, it should throw ArgumentException
        [Fact]
        public void AddPerson_WithPersonNameIsNull_ShouldThrowArgumentException()
        {
            PersonAddRequest personAddRequest = A.PersonAddRequest.WithName(null);

            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
        }

        // Requirement:
        // When we supply properly person details, it should insert the person into the persons list;
        // and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public void AddPerson_WithProperPersonAddRequest_ShouldReturnPersonResponseWithNewlyPersonID()
        {
            // Arrange
            PersonAddRequest personAddRequest = A.PersonAddRequest.Build();

            // Act
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            List<PersonResponse> peopleResponse = _personService.GetPeople();

            // Assert
            Assert.True(personResponse.PersonID != Guid.Empty);
            Assert.Contains(personResponse, peopleResponse);
        }

        #endregion

        #region GetPersonByPersonID

        // If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonID_WithNullPersonID_ShouldReturnNull()
        {
            // Arrange
            Guid? personID = null;
    
            // Act
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personID);

            // Assert
            Assert.Null(personResponse);
        }

        // If we supply properly a person id, it shoudl return a valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonID_WithProperlyPersonID_ShouldReturnPersonDetails()
        {
            // Arrange
            CountryResponse countryResponse = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);

            PersonAddRequest personAddRequest = A.PersonAddRequest
                                                 .WithCountryId(countryResponse.CountryID);

            // Act
            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);

            // Assert            
            Assert.NotNull(person_response_from_get);
            Assert.Equal(person_response_from_get, person_response_from_add);
        }

        #endregion

        #region GetPeople

        // The GetPeople should return an empty list as default
        [Fact]
        public void GetPeople_Always_ReturnsEmptyList()
        {
            List<PersonResponse> people = _personService.GetPeople();

            Assert.Empty(people);
        }

        // First, We will add few persons, and then when we call GetPeople(), it should return
        // the same persons that were added
        [Fact]
        public void GetPeople_WithAddFewPersons_ShouldReturnsTheSameFewPersons()
        {
            // Arrange            
            CountryResponse country_response_from_add = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);
            List<PersonAddRequest> personAddRequests = CreatePersonAddRequestList(country_response_from_add);
            
            // Act
            List<PersonResponse> people_response_from_add = personAddRequests.Select(personAddRequest => _personService.AddPerson(personAddRequest)).ToList();
            List<PersonResponse> people_response_from_get = _personService.GetPeople();

            // print people_response_from_add
            _testOutputHelper.WriteLine("Expected: ");
            people_response_from_add.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            // print people_response_from_get
            _testOutputHelper.WriteLine("Actual: ");
            people_response_from_get.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            // Assert
            people_response_from_add.ForEach(peron_response_from_add =>
            {
                Assert.Contains(peron_response_from_add, people_response_from_get);
            });
        }

        #endregion


        #region GetFilteredPersons

        // If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_WithEmptyPersonName_ReturnsAllPersonsSearched()
        {
            // Arrange
            var country_response_from_add = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);
            var personAddRequests = CreatePersonAddRequestList(country_response_from_add);

            // 
            List<PersonResponse> people_response_from_add = personAddRequests.Select(personAddRequest => _personService.AddPerson(personAddRequest)).ToList();            
         
            // print people_response_from_add
            _testOutputHelper.WriteLine("Expected: ");
            people_response_from_add.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            string searchText = string.Empty;

            List<PersonResponse> people_response_from_search = 
                _personService.GetFilteredPersons(nameof(Person.PersonName), searchText);

            // print people_response_from_get
            _testOutputHelper.WriteLine("Actual: ");
            people_response_from_search.ForEach(person => _testOutputHelper.WriteLine(person?.ToString()));

            // Assert              
            people_response_from_add.ForEach(person_from_add =>
            {
                Assert.Contains(person_from_add, people_response_from_search);
            });
        }

        // First we will add few persons; and then we will search based on person name with some search string.
        // It should return the matching persons.
        [Fact]
        public void GetFilteredPersons_SearchByPersonName_ReturnsMachingPerson()
        {
            // Arrange
            var country_response_from_add = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);
            var personAddRequests = CreatePersonAddRequestList(country_response_from_add);

            string searchBy = nameof(Person.PersonName);
            string searchText = "an";

            List<PersonResponse> people_response_from_add = personAddRequests.Select(personAddRequest => _personService.AddPerson(personAddRequest)).ToList();
         
            // print people_response_from_add
            _testOutputHelper.WriteLine("Expected: ");
            people_response_from_add.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            List<PersonResponse?> people_response_from_search = _personService.GetFilteredPersons(searchBy, searchText);

            // print people_response_from_get
            _testOutputHelper.WriteLine("Actual Searched: ");
            people_response_from_search.ForEach(person => _testOutputHelper.WriteLine(person?.ToString()));
                        
            // Assert              
            Assert.NotNull(people_response_from_search);
            people_response_from_add.ForEach(person_from_add =>
            {
                if (person_from_add.PersonName is not null)
                {
                    if (person_from_add.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_from_add, people_response_from_search);
                    }
                }
            });
        }

        #endregion

        #region GetSortedPersons

        // When we sort based on PersonName is DESC, it should return persons list in descending on PersonName
        [Fact]
        public void GetSortedPersons()
        {
            // Arrange
            var country_response_from_add = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);
            var personAddRequests = CreatePersonAddRequestList(country_response_from_add);

            string sortBy = nameof(PersonResponse.Age);
            SortOrderOptions sortOrder = SortOrderOptions.DESC;

            List<PersonResponse> people_response_from_add =
                personAddRequests
                    .Select(personAddRequest => _personService.AddPerson(personAddRequest)).ToList();                    

            // print people_response_from_add
            _testOutputHelper.WriteLine("Expected before ordering: ");
            people_response_from_add.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            people_response_from_add =
                people_response_from_add.OrderByProperty(sortBy, sortOrder).ToList();

            _testOutputHelper.WriteLine("Expected afterr ordering ascending: ");
            people_response_from_add.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            List<PersonResponse> people_response_from_get = _personService.GetPeople();
            List<PersonResponse> people_response_from_sort = 
                _personService.GetSortedPersons(people_response_from_get, sortBy, sortOrder);
            
            // print people_response_from_get
            _testOutputHelper.WriteLine("Actual Sorted By PersonName: ");
            people_response_from_sort.ForEach(person => _testOutputHelper.WriteLine(person.ToString()));

            // Assert              
            Assert.NotNull(people_response_from_sort);
            Assert.Equal(people_response_from_add, people_response_from_sort);
        }

        #endregion

        #region UpdatePerson

        // When we supply null as PersonUpdateRequest, it shuold throw ArgumentNullException
        [Fact]
        public void UpdatePerson_WithNullPerson_ShowThrowArgumentNullException()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            // Action
            Action action = () => _personService.UpdatePerson(personUpdateRequest);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        // When we supply invalid person id as PersonUpdateRequest, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_WithInvalidPersonID_ShowThrowArgumentNullException()
        {
            // Arrange
            Guid personIdDoesNotExist = Guid.NewGuid();
            PersonUpdateRequest? personUpdateRequest = A.PersonUpdateRequest.WithId(personIdDoesNotExist);

            // Action
            Action action = () => _personService.UpdatePerson(personUpdateRequest);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        // When we supply person name is null as PersonUpdateRequest, it should throw ArgumentException
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void UpdatePerson_WithInvalidPersonName_ShowThrowArgumentNullException(string personName)
        {
            // Arrange                                   
            PersonUpdateRequest? personUpdateRequest = CreatePersonUpdateRequest();
            personUpdateRequest.PersonName = personName;

            // Action
            Action action = () => _personService.UpdatePerson(personUpdateRequest);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        // First, add a new person and try to update the person name and email
        [Theory]
        [InlineData("An", "annogo123@gmail.com")]
        [InlineData("Di", "dinogo1234@gmail.com")]
        public void UpdatePerson_PersonFullDetailsUpdation_ShouldReturnPersonNameAndEmailUpdated(
            string personName,
            string email)
        {
            // Arrange - add and get personUpdateReqest
            PersonUpdateRequest personUpdateRequest = CreatePersonUpdateRequest();
            
            _testOutputHelper.WriteLine("Person full details updation after creating person update request: ");
            _testOutputHelper.WriteLine(personUpdateRequest.ToString());

            // updating fields
            personUpdateRequest.PersonName = personName;
            personUpdateRequest.Email = email;

            // Act
            _testOutputHelper.WriteLine("After updating person and getting person response: ");
            PersonResponse person_response_updated = _personService.UpdatePerson(personUpdateRequest);
            _testOutputHelper.WriteLine(person_response_updated.ToString());


            _testOutputHelper.WriteLine("Getting person response based on given id of person updated: ");
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_updated.PersonID);
            _testOutputHelper.WriteLine(person_response_updated.ToString());

            // Assert
            Assert.Equal(person_response_updated, person_response_from_get);            
        }

        #endregion

        #region DeletePerson

        // If you supply an valid PersonID, it should return true
        [Fact]
        public void DeletePerson_ValidPersonID_ShouldReturnFalse()
        {
            CountryResponse countryResponse = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);
            Guid countryId = IsGuidNullOrEmpty(countryResponse.CountryID) ? PersonAddRequestBuilder.TEST_PERSON_ADD_REQUEST_COUNTRY_ID : countryResponse.CountryID;
            PersonAddRequestBuilder personAddRequestBuilder = A.PersonAddRequest.WithCountryId(countryId);
            PersonResponse person_response_from_add = GetPersonResponseByAddPersonRequest(personAddRequestBuilder);

            bool isDeleted = _personService.DeletePerson(person_response_from_add.PersonID);

            Assert.True(isDeleted);
        }

        // If you supply an invalid PersonID, it should return false
        [Fact]
        public void DeletePerson_InvalidPersonID_ShouldReturnFalse()
        {
            CountryResponse countryResponse = GetCountryResponseByAddingCountryAddRequest(A.CountryAddRequest);
            Guid countryId = IsGuidNullOrEmpty(countryResponse.CountryID) ? PersonAddRequestBuilder.TEST_PERSON_ADD_REQUEST_COUNTRY_ID : countryResponse.CountryID;
            PersonAddRequestBuilder personAddRequestBuilder = A.PersonAddRequest.WithCountryId(countryId);
            PersonResponse person_response_from_add = GetPersonResponseByAddPersonRequest(personAddRequestBuilder);

            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

            Assert.False(isDeleted);
        }

        #endregion
    }
}
