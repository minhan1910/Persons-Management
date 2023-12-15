using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        public bool IsCountryNameDuplicated(string? otherCountryName)
        {
            if (otherCountryName is null) 
                return false;

            if (CountryName is null)
                return false;
            
            return this.CountryName.Equals(otherCountryName, StringComparison.Ordinal);
        }

        public static Func<Country, bool> IsCountryNameDuplicated(Country otherCountry)
        {
            return country => country.IsCountryNameDuplicated(otherCountry.CountryName);
        }
    }
}