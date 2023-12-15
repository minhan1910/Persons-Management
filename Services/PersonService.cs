using Entities;
using ServiceContracts;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Enums;
using ServiceContracts.Utils;
using Services.Extensions;
using Services.Helpers;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Services
{
    public class PersonService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        //public PersonService(ICountriesService countriesService)
        //{
        //    _persons = new List<Person>();
        //    _countriesService = countriesService;
        //}

        public PersonService(ICountriesService countriesService, bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = countriesService;

            if (initialize)
            {
                _persons.AddRange(new List<Person>
                {
                    new Person
                    {
                        PersonID = Guid.Parse("55002f4b-c88c-40de-970f-1be17ed798a8"),
                        PersonName = "Wildon",
                        Email = "wtabary0@smugmug.com",
                        DateOfBirth = new DateTime(1990, 04, 12),
                        Gender = "Male",
                        Address = "1151 Fallview Street",
                        ReceiveNewsLetters = true,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("92cb37e1-8c27-4812-8e9d-b783f8ed6f4d"),
                        PersonName = "Shane",
                        Email = "sgreenhough1@sphinn.com",
                        DateOfBirth = new DateTime(1990, 02, 03),
                        Gender = "Female",
                        Address = "2 Hauk Point",
                        ReceiveNewsLetters = true,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),                        
                    },                
                    new Person
                    {
                        PersonID = Guid.Parse("605b3869-27a3-4ad9-92ad-cef416632d08"),
                        PersonName = "Sancho",
                        Email = "snewbury2@symantec.com",
                        DateOfBirth = new DateTime(1992, 07, 21),
                        Gender = "Male",
                        Address = "6 Redwing Way",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),
                    },
                    new Person
                    {
                        PersonName = "Clerissa",
                        Email = "cizon3@reuters.com",
                        DateOfBirth = new DateTime(1999, 08, 22),
                        Gender = "Female",
                        Address = "95 Schmedeman Park",
                        ReceiveNewsLetters = true,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),
                    },
                    new Person
                    {                    
                        PersonID = Guid.Parse("605b3869-27a3-4ad9-92ad-cef416632d08"),
                        PersonName = "Ad",
                        Email = "ahannigan4@hostgator.com",
                        DateOfBirth = new DateTime(1996, 02, 04),
                        Gender = "Male",
                        Address = "5 Graedel Center",
                        ReceiveNewsLetters = true,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db")
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("3c7707e9-6db0-4b0b-9d3d-bc254b4fe834"),
                        PersonName = "Sly",
                        Email = "storrijos5@printfriendly.com",
                        DateOfBirth = new DateTime(1990, 09, 17),
                        Gender = "Male",
                        Address = "00933 Huxley Drive",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("5cb48004-54be-4ef4-a15d-1df6d37cc943"),
                        PersonName = "Doralia",
                        Email = "dmungin6@icq.com",
                        DateOfBirth = new DateTime(1991, 02, 17),
                        Gender = "Female",
                        Address = "02458 Independence Way",
                        ReceiveNewsLetters = true,                        
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse(" df75ff6d-7502-46b9-a719-3dca9e78bc3e"),
                        PersonName = "Parker",
                        Email = "proyson7@msn.com",
                        DateOfBirth = new DateTime(1997, 01, 23),
                        Gender = "Male",
                        Address = "93 Ohio Drive",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db")
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("a715b2e5-1f27-4a6c-a540-ef5bd6858503"),
                        PersonName = "Catharina",
                        Email = "cjannex8@tinypic.com",
                        DateOfBirth = new DateTime(1994, 09, 05),
                        Gender = "Female",
                        Address = "82 Westport Alley",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("ead5a8e9-f019-4d87-9658-fe0673d220a7"),
                        PersonName = "Allister",
                        Email = "aphillot9@epa.gov",
                        DateOfBirth = new DateTime(1993, 05, 30),
                        Gender = "Male",
                        Address = "45 Sunnyside Way",
                        ReceiveNewsLetters = true,
                        CountryID = Guid.Parse("335d5937-6111-4787-9d75-b68d27b68587"),
                    }
            });
            }
        }


        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
            {
                throw new ArgumentNullException(nameof(PersonAddRequest));
            }

            // Validate PersonName
            //if (string.IsNullOrEmpty(personAddRequest.PersonName))
            //{
            //    throw new ArgumentException("Person can't be blank");
            //}

            // Model Validations
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();

            // Add new Peson into list
            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            var personResposne = person.ToPersonResponse();
            personResposne.CountryName = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;

            return personResposne;
        }

        public List<PersonResponse> GetPeople()
        {
            var s = _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
            return s;
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID is null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(person => person.PersonID.Equals(personID));

            if (person is null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(person);
        }

        #region GetFilteredPersons

        private Dictionary<string, Func<string, Predicate<Person>>> searchByAndActions =
            new Dictionary<string, Func<string, Predicate<Person>>>
        {
            { nameof(Person.PersonName), FilterByPersonName },
            { nameof(Person.Email), FilterByEmail },
            { nameof(Person.DateOfBirth), FilterByDateOfBirth },            
            { nameof(Person.Gender), FilterByGender },
            { nameof(Person.CountryID), FilterByCountry },
            { nameof(Person.Address), FilterByAddress },
        };

        private static Func<string, Predicate<Person>> FilterByPersonName => 
            searchString =>  person =>
            person.PersonName is not null &&
            person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase);

        private static Func<string, Predicate<Person>> FilterByEmail =>
           searchString =>
           person =>
           person.Email is not null &&
           person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase);           

        private static Func<string, Predicate<Person>> FilterByDateOfBirth =>
            searchString =>
            person =>
            person.DateOfBirth is not null &&
            person.DateOfBirth.Value
                .ToString("dd MMMM yyyy")
                .Contains(searchString, StringComparison.OrdinalIgnoreCase);

        private static Func<string, Predicate<Person>> FilterByGender =>
            searchString =>
            person =>
            person.Gender is not null &&
            person.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase);

        private static Func<string, Predicate<Person>> FilterByCountry =>
            searchString =>
            person =>
            person.CountryID is not null && 
            person.Country is not null &&
            person.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase);

        private static Func<string, Predicate<Person>> FilterByAddress => 
            searchString => 
            person => 
            person.Address is not null && 
            person.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase);

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
            {
                return GetPeople();
            }

            Func<Person, bool>? searchFunc = person => string.IsNullOrEmpty(searchBy);

            if (searchByAndActions.TryGetValue(searchBy, out Func<string, Predicate<Person>>? action))
            {   
                Func<Person, bool> tempFunc = person => action(searchString)(person);
                searchFunc = tempFunc;
            } 
            else
            {
                // temporary throws exception
                throw new ArgumentException("Search by propery must have into Person model.");
            }

            var personsMatchedSearchString = _persons.Where(searchFunc)
                                                     .Select(MapToPersonResponse).ToList();

            return personsMatchedSearchString;
        }



        #endregion

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, 
                                                     string sortBy, 
                                                     SortOrderOptions sortOrder)
        {                         
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return allPersons;
            }

            //IEnumerable<PersonResponse> sortedPersons = sortBy 
            //switch
            //{
            //    // (nameof(Person.PersonName), SortOrderOptions.ASC) 
            //    // => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase),
            //    // (nameof(Person.PersonName), SortOrderOptions.DESC)
            //    //=> allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase),
            //    // (nameof(Person.Email), SortOrderOptions.ASC)
            //    //=> allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase),
            //    // (nameof(Person.Email), SortOrderOptions.DESC)
            //    //=> allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase),

            //    // refactored 
            //    // but we have passed into sortOrder each method and not dynamic sortBy so we need to refactor more once
            //     nameof(PersonResponse.PersonName) 
            //     => allPersons.OrderByProperty(personFromList => personFromList.PersonName, sortOrder, StringComparer.OrdinalIgnoreCase),

            //     nameof(PersonResponse.Email)
            //     => allPersons.OrderByProperty(personFromList => personFromList.Email, sortOrder, StringComparer.OrdinalIgnoreCase),

            //    nameof(PersonResponse.DateOfBirth)
            //    => allPersons.OrderByProperty(personFromList => personFromList.DateOfBirth, sortOrder),

            //    nameof(PersonResponse.Age) 
            //    => allPersons.OrderByProperty(personFromList => personFromList.Age, sortOrder),

            //    nameof(PersonResponse.Gender)
            //    => allPersons.OrderByProperty(personFromList => personFromList.Gender, sortOrder, StringComparer.OrdinalIgnoreCase),

            //    nameof(PersonResponse.CountryName)
            //    => allPersons.OrderByProperty(personFromList => personFromList.CountryName, sortOrder, StringComparer.OrdinalIgnoreCase),

            //    nameof(PersonResponse.ReceiveNewsLetters)
            //    => allPersons.OrderByProperty(personFromList => personFromList.ReceiveNewsLetters, sortOrder),

            //    _ => allPersons
            //};

            return allPersons.OrderByProperty(sortBy, sortOrder).ToList();
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            // Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? personFromList = _persons.FirstOrDefault(person => person.PersonID == personUpdateRequest.PersonID);            

            if (personFromList is null)
            {
                throw new ArgumentException("Given personUpdated id doesn't exist");
            }

            Person? personUpdated = PropertiesHandler<PersonUpdateRequest, Person>.Copy(personUpdateRequest, personFromList);

            return ConvertPersonToPersonResponse(personUpdated!);
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID.HasValue == false)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? personBasedOnGivenId = _persons.FirstOrDefault(person => person.PersonID == personID.Value);            
            
            if (personBasedOnGivenId is null)
            {
                return false;
            }

            bool isDeleted = _persons.Remove(personBasedOnGivenId);

            return isDeleted;
        }

        private Func<Person, PersonResponse> MapToPersonResponse => person => ConvertPersonToPersonResponse(person);
    }
}
