using Entities;
using ServiceContracts.Utils;
using System.Security.Cryptography.X509Certificates;

namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Represents DTO class that it is used as return type of most methods of Persons Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }        
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the other parameter object        
        /// </summary>
        /// <param name="obj">The PersonResponse Object to compare</param>
        /// <returns>True or False, indicating whether all person details with 
        /// specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }

            return PropertiesHandler<PersonResponse, PersonResponse>.Compare(this, obj as PersonResponse);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(PersonID);
            hash.Add(PersonName);
            hash.Add(Email);
            hash.Add(DateOfBirth);
            hash.Add(Gender);
            hash.Add(CountryID);
            hash.Add(CountryName);
            hash.Add(Address);
            hash.Add(ReceiveNewsLetters);
            hash.Add(Age);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            return $"{nameof(PersonID)}: {PersonID}, {nameof(PersonName)}: {PersonName}, {nameof(Email)}: {Email}, " +
                $"{nameof(DateOfBirth)}: {DateOfBirth?.ToString("dd MMMM yyyy")}, {nameof(Gender)}: {Gender}, " +
                $"{nameof(CountryID)}: {CountryID}, {nameof(CountryName)}: {CountryName}, {nameof(Address)}: {Address}, " +
                $"{nameof(ReceiveNewsLetters)}: {ReceiveNewsLetters}, {nameof(Age)}: {Age}";
        }

        // for unit test
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return PropertiesHandler<PersonResponse, PersonUpdateRequest>.Copy(this, new PersonUpdateRequest());
        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Person class into PersonResponse class                
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Returns the converted PersonResponse object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            var personResponseFromPerson = PropertiesHandler<Person, PersonResponse>.Copy(person, new PersonResponse());

            if (personResponseFromPerson == null)
            {
                return new PersonResponse();
            }

            personResponseFromPerson.Age = (person.DateOfBirth != null) ? 
                Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null;

            return personResponseFromPerson;
        }
    }
}