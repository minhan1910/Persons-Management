using ServiceContracts.DTO.PersonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests.Factory.Country
{
    public class PersonUpdateRequestBuilder : BaseTest<PersonUpdateRequestBuilder, PersonUpdateRequest>
    {
        #region TEST CONSTANTS        
        private static readonly Guid TEST_PERSON_UPDATE_REQUEST_PERSON_ID = Guid.NewGuid();
        #endregion

        public Guid PersonId { get; private set; }
        public string PersonName { get; private set; }

        public static implicit operator PersonUpdateRequest(PersonUpdateRequestBuilder builder) => builder.Build();

        public PersonUpdateRequestBuilder() 
        {
            PersonId = TEST_PERSON_UPDATE_REQUEST_PERSON_ID;
        }

        private PersonUpdateRequestBuilder(PersonUpdateRequestBuilder otherPersonUpdateRequestBuilder)
        {
            PersonId = otherPersonUpdateRequestBuilder.PersonId;
        }

        public override PersonUpdateRequest Build()
        {
            return new PersonUpdateRequest
            {
                PersonID = PersonId,
            };
        }

        protected override PersonUpdateRequestBuilder CopyAndUpdate(PersonUpdateRequestBuilder personUpdateRequestBuilder)
        {
            return new PersonUpdateRequestBuilder(personUpdateRequestBuilder);
        }

        public PersonUpdateRequestBuilder WithId(Guid id)
        {
            PersonId = id;
            return CopyAndUpdate(this);
        }

        public PersonUpdateRequestBuilder WithPersonName(string personName)
        {

            PersonName = personName;
            return CopyAndUpdate(this);
        }
    }
}
