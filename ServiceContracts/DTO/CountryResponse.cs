using Entities;
using ServiceContracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type of most of CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        // It compares the current object to antother object of CountryResponse 
        // type and returns true, if both values are the same; otherwise returns false
        public override bool Equals(object? obj)
        {
            //return obj is CountryResponse response &&
            //       CountryID.Equals(response.CountryID) &&
            //       CountryName == response.CountryName;

            if (obj is null || obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            return PropertiesHandler<CountryResponse, CountryResponse>.Compare(this, obj as CountryResponse);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CountryID, CountryName);
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName,
            };
        }
    }
}
