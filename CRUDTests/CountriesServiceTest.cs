using CRUDTests.Factory;
using CRUDTests.Factory.Country;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;        

        public CountriesServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region Test Helper Method

        public List<CountryAddRequest> CreateCountryAddRequestList() => new List<CountryAddRequest>
        {
            A.CountryAddRequest.WithCountryName("VN"),
            A.CountryAddRequest.WithCountryName("German"),
            A.CountryAddRequest.WithCountryName("Finland"),
        };

        #endregion

        #region AddCountry

        /* When CountryAddRequest is null, it should throw ArgumentNullException */
        [Fact]
        public void AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        /* When CountryName is null, it should throw ArgumentNullException */
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest request = A.CountryAddRequest.WithCountryName(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        /* When CountryName is duplicated, it should throw ArgumentException */
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            // Arrange
            CountryAddRequest? request1 = A.CountryAddRequest.Build();
            CountryAddRequest? request2 = A.CountryAddRequest.Build();

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        /**
         * When you supply proper country name, 
         * it should insert (add) the country to the existing list of countries
         */
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange
            //CountryAddRequest? request = new CountryAddRequest() { CountryName = "Germany" };
            var request = A.CountryAddRequest.Build();

            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            // Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Contains(countryResponse, countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries

        /* The list of countries should be empty by default (before adding any country) */
        [Fact]
        public void GetAllCountries_EmptyList()
        {            
            // Acts
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(actual_country_response_list);
        }

        /* The list of countries should be empty by default (before adding any country) */
        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = CreateCountryAddRequestList();

            List<CountryResponse> countries_list_from_add_country =
                country_request_list.Select(country_request => _countriesService.AddCountry(country_request))
                .ToList();

            //country_request_list.ForEach(country_request =>
            //{
            //    CountryResponse country_from_add_country = _countriesService.AddCountry(country_request);
            //    countries_list_from_add_country.Add(country_from_add_country);
            //});            

            // Actutal countries from service
            List<CountryResponse> actual_countries_response_list = _countriesService.GetAllCountries();

            // Assert
            countries_list_from_add_country.ForEach(expected_country =>
            {
                Assert.Contains(expected_country, actual_countries_response_list);
            });
        }

        #endregion

        #region GetCountryByCountryID

        /* If we supply null as CountryID, it should return null as CountryResponse */
        [Fact]
        public void GetCountryByCountryID_WithNullCountryID_CheckCountryResponseIsNull()
        {
            // Arrange
            Guid? countryID = null;

            // Assert
            // Act
            Assert.Null(_countriesService.GetCountryByCountryID(countryID));
        }

        /* If we supply a valid country id, it should return the matching country details as CountryRespone type */
        [Fact]
        public void GetCountryByCountryID_WithProperlyCountryID_ShouldReturnCountryDetails()
        {
            // Arrange
            // Each test case make Empty List so we need to AddCountry before implementing this test
            CountryAddRequest countryAddRequest = A.CountryAddRequest.Build();
            CountryResponse expected_country_response = _countriesService.AddCountry(countryAddRequest);

            // Acts
            CountryResponse? actual_country_response = 
                _countriesService.GetCountryByCountryID(expected_country_response.CountryID);

            // Assert
            Assert.Equal(expected_country_response, actual_country_response);
        }

        /* If we sullpy country id which is not existed on list, it should return null as CountryResponse type */
        [Fact]
        public void GetCountryByCountryID_WithCountryIDNotExisted_CheckCountryResponseIsNull()
        {
            CountryAddRequest countryAddRequest = A.CountryAddRequest.Build();

            CountryResponse expected_country_response = _countriesService.AddCountry(countryAddRequest);

            Guid? country_id_is_not_existed = Guid.NewGuid();
            // Acts
            CountryResponse? actual_country_response = _countriesService.GetCountryByCountryID(country_id_is_not_existed);

            // Assert
            Assert.Null(actual_country_response);
        }

        #endregion

    }
}
