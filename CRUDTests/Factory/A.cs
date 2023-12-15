using CRUDTests.Factory.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests.Factory
{
    public static class A
    {
        public static CountryAddRequestBuilder CountryAddRequest => new CountryAddRequestBuilder();
        public static PersonAddRequestBuilder PersonAddRequest => new PersonAddRequestBuilder();
        public static PersonUpdateRequestBuilder PersonUpdateRequest => new PersonUpdateRequestBuilder();
    }
}
