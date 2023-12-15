using Entities;
using ServiceContracts.Enums;
using ServiceContracts.Utils;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Acts as DTO for inserting new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should    be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender must be included")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage = "Country must be included")]
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>person object as Person type</returns>
        public Person ToPerson()
        {
            return PropertiesHandler<PersonAddRequest, Person>.Copy(this, Person.Create())!;
            //return new Person()
            //{
            //    PersonName = PersonName,
            //    Email = Email,
            //    DateOfBirth = DateOfBirth,
            //    Gender = Convert.ToString(Gender),
            //    CountryID = CountryID,
            //    Address = Address,
            //    ReceiveNewsLetters = ReceiveNewsLetters
            //};
        }
    }
}