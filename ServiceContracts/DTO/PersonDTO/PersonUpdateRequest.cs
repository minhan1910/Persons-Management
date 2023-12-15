using Entities;
using ServiceContracts.Enums;
using ServiceContracts.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Represents the DTO class that contains the person details to update    
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person ID can't be blank")]
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should    be a valid email")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>person object as Person type</returns>
        public Person ToPerson()
        {
            return PropertiesHandler<PersonUpdateRequest, Person>.Copy(this, Person.Create())!;
        }
        public override string ToString()
        {
            return $"{nameof(PersonID)}: {PersonID}, {nameof(PersonName)}: {PersonName}, {nameof(Email)}: {Email}, " +
                $"{nameof(DateOfBirth)}: {DateOfBirth?.ToString("dd MMMM yyyy")}, {nameof(Gender)}: {Gender}, " +
                $"{nameof(CountryID)}: {CountryID}, {nameof(Address)}: {Address}, " +
                $"{nameof(ReceiveNewsLetters)}: {ReceiveNewsLetters}";
        }

    }
}
