using CRUDTests.Factory.Time;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests.Factory.Country
{
    //PersonAddRequest? personAddRequest = new()
    //{
    //    PersonName = "An",
    //    Email = "An@gmail.com",
    //    DateOfBirth = DateTime.Parse("2000-01-01"),
    //    ReceiveNewsLetters = true,
    //    CountryID = Guid.NewGuid(),
    //    Address = "address",
    //    Gender = GenderOptions.Male
    //};
    public class PersonAddRequestBuilder : BaseTest<PersonAddRequestBuilder, PersonAddRequest>
    {
        #region Test Data Default        
        public static readonly Guid TEST_PERSON_ADD_REQUEST_COUNTRY_ID = Guid.NewGuid();
        public static readonly DateTime TEST_PERSON_ADD_REQUEST_DATE_OF_BIRTH = SystemTime.Now;
        public const GenderOptions TEST_PERSON_ADD_REQUEST_GENDER = GenderOptions.Male;
        public const bool TEST_PERSON_ADD_REQUEST_RECEIVE_NEWS_LETTERS = true;
        public const string TEST_PERSON_ADD_REQUEST_ADDRESS = "Address";
        public const string TEST_PERSON_ADD_REQUEST_EMAIL = "An@gmail.com";
        public const string TEST_PERSON_ADD_REQUEST_PERSON_NAME = "An";
        #endregion

        #region Public Properties        
        public string PersonName { get; private set; }
        public string Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public GenderOptions Gender { get; private set; }
        public Guid CountryID { get; private set; }
        public string Address { get; private set; }
        public bool ReceiveNewsLetters { get; private set; }
        #endregion

        public PersonAddRequestBuilder()
        {            
            PersonName = TEST_PERSON_ADD_REQUEST_PERSON_NAME;
            Email = TEST_PERSON_ADD_REQUEST_EMAIL;
            DateOfBirth = TEST_PERSON_ADD_REQUEST_DATE_OF_BIRTH;
            Gender = TEST_PERSON_ADD_REQUEST_GENDER;
            CountryID = TEST_PERSON_ADD_REQUEST_COUNTRY_ID;
            Address = TEST_PERSON_ADD_REQUEST_ADDRESS;
            ReceiveNewsLetters = TEST_PERSON_ADD_REQUEST_RECEIVE_NEWS_LETTERS;
        }

        private PersonAddRequestBuilder(PersonAddRequestBuilder anotherBuilder)
        {            
            PersonName = anotherBuilder.PersonName;
            Email = anotherBuilder.Email;
            DateOfBirth = anotherBuilder.DateOfBirth;
            Gender = anotherBuilder.Gender;
            CountryID = anotherBuilder.CountryID;
            Address = anotherBuilder.Address;
            ReceiveNewsLetters = anotherBuilder.ReceiveNewsLetters;
        }

        public static implicit operator PersonAddRequest(PersonAddRequestBuilder builder) => builder.Build();

        protected override PersonAddRequestBuilder CopyAndUpdate(PersonAddRequestBuilder anotherBuilder)
        {
            return new PersonAddRequestBuilder(anotherBuilder);
        }

        public override PersonAddRequest Build()
        {
            return new PersonAddRequest
            {                
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters,
            };
        }

        public PersonAddRequestBuilder WithName(string personName)
        {
            PersonName = personName;
            return CopyAndUpdate(this);
        }

        public PersonAddRequestBuilder WithEmail(string email)
        {
            Email = email;
            return CopyAndUpdate(this);
        }

        public PersonAddRequestBuilder WithCountryId(Guid countryID)
        {
            CountryID = countryID;
            return CopyAndUpdate(this);
        }

        public PersonAddRequestBuilder WithGender(GenderOptions gender)
        {
            Gender = gender;
            return CopyAndUpdate(this);
        }

    }
}
