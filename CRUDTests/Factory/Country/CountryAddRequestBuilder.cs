using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests.Factory.Country
{
    public class CountryAddRequestBuilder : BaseTest<CountryAddRequestBuilder, CountryAddRequest>
    {
        public const string TEST_COUNTRY_ADD_REQUEST_COUNTRYNAME = "VN";
        public string CountryName { get; private set; }

        public static implicit operator CountryAddRequest(CountryAddRequestBuilder builder)
        {
            return builder.Build();
        }

        public CountryAddRequestBuilder() 
        {
            CountryName = TEST_COUNTRY_ADD_REQUEST_COUNTRYNAME;
        }

        private CountryAddRequestBuilder(CountryAddRequestBuilder anotherCountryAddRequest)
        {
            CountryName = anotherCountryAddRequest.CountryName;
        }

        public CountryAddRequestBuilder WithCountryName(string name)
        {
            CountryName = name;
            return CopyAndUpdate(this);
        }

        protected override CountryAddRequestBuilder CopyAndUpdate(CountryAddRequestBuilder anotherBuilder)
        {
            return new CountryAddRequestBuilder(anotherBuilder);
        }

        public override CountryAddRequest Build()
        {
            return new CountryAddRequest
            {
                CountryName = CountryName
            };
        }
    }
}
