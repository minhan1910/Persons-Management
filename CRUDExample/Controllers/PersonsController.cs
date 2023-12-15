using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonsService personsService, 
                                 ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("/")]
        [Route("[action]")]
        public IActionResult Index(string searchBy, 
                                   string? searchString, 
                                   string sortBy = nameof(PersonResponse.PersonName),
                                   SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>
            {
                [nameof(PersonResponse.PersonName)] = "Person Name",
                [nameof(PersonResponse.Email)] = "Email",
                [nameof(PersonResponse.DateOfBirth)] = "Date of Birth",
                [nameof(PersonResponse.CountryID)] = "Country",
                [nameof(PersonResponse.Address)] = "Address",
                [nameof(PersonResponse.Gender)] = "Gender",
            };

            ViewBag.CurrentSearchBy = searchBy; 
            ViewBag.CurrentSearchString = searchString;

            // search
            List<PersonResponse> personResponses = _personsService.GetFilteredPersons(searchBy, searchString);

            // sort
            List<PersonResponse> sortedAllPersons = _personsService.GetSortedPersons(personResponses, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString(); 

            return View(sortedAllPersons);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            List<ServiceContracts.DTO.CountryResponse> allCountries = _countriesService.GetAllCountries();
            ViewBag.Countries = allCountries.Select(country => new SelectListItem
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString()
            });
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> allCountries = _countriesService.GetAllCountries();
                ViewBag.Countries = allCountries;

                List<string> errorMsges = ModelState.Values
                                                  .SelectMany(x => x.Errors)
                                                  .Select(error => error.ErrorMessage)
                                                  .ToList();

                ViewBag.Errors = errorMsges;

                return View();
            }

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            // make another requests to ~/persons/index
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Edit(Guid personId)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personId);

            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            List<CountryResponse> allCountries = _countriesService.GetAllCountries();
            ViewBag.Countries = allCountries.Select(country => new SelectListItem
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString()
            });

            return View(personResponse.ToPersonUpdateRequest());
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personBasedOnGivenId = _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personBasedOnGivenId is null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid == false)
            {
                List<CountryResponse> allCountries = _countriesService.GetAllCountries();
                ViewBag.Countries = allCountries;

                List<string> errorMsges = ModelState.Values
                                                  .SelectMany(x => x.Errors)
                                                  .Select(error => error.ErrorMessage)
                                                  .ToList();

                ViewBag.Errors = errorMsges;
                return View(personUpdateRequest);
            }

            PersonResponse personUpdated = _personsService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Delete(Guid personId)
        {
            PersonResponse? personBasedOnGivenId = _personsService.GetPersonByPersonID(personId);

            if (personBasedOnGivenId is null)
            {
                return RedirectToAction("Index");
            }

            return View(personBasedOnGivenId.ToPersonUpdateRequest());
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personBasedOnGivenId = _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personBasedOnGivenId is null)
            {
                return RedirectToAction("Index");
            }

            bool isPersonDeleted = _personsService.DeletePerson(personUpdateRequest.PersonID);

            return RedirectToAction("Index");
        }
    }
}
