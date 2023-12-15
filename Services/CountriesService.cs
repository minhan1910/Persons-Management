using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private readonly List<Country> _db;
        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext personsDbContext) 
        {
            _db = personsDbContext;

            //if (initialize)
            //{
            //    _db.AddRange(new List<Country>
            //    {
            //        new Country { CountryID = Guid.Parse("0dbe6d8c-c5aa-44c9-bae2-94121a64f3db"), CountryName = "VietNam" },
            //        new Country { CountryID = Guid.Parse("335d5937-6111-4787-9d75-b68d27b68587"), CountryName = "Germany" },
            //        new Country { CountryID = Guid.Parse("4deb4179-36f5-4a74-994a-53dbfed3673b"), CountryName = "France" },
            //        new Country { CountryID = Guid.Parse("faba29bd-15d5-462d-b312-9df3cf8ffa5e"), CountryName = "Spain" },
            //        new Country { CountryID = Guid.Parse("43e0ba26-e985-46eb-a8a1-398992ea3ee3"), CountryName = "Finland" },
            //    });
            //}
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest is null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName is null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest.CountryName));
            }

            if (_db.Any(country => country.IsCountryNameDuplicated(countryAddRequest.CountryName)))
            {
                throw new ArgumentException("Given country name already exists");
            }         

            Country newCountry = countryAddRequest.ToCountry();            
            newCountry.CountryID = Guid.NewGuid();
            _db.Add(newCountry);
                
            return newCountry.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {            
            return _db.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryId)
        {
            if (countryId is null)
            {
                return null;
            }

            Country? countryFromList = _db.FirstOrDefault(country =>  country.CountryID == countryId);

            if (countryFromList is null)
            {
                return null;
            }

            return countryFromList.ToCountryResponse();
        }
    }    
}