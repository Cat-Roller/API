using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_App_Web_API_.Dto;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;
using Pokemon_Review_App_Web_API_.Repository;

namespace Pokemon_Review_App_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private IMapper _mapper;
        private ICountryRepository _CountryRepository;

        public CountryController(ICountryRepository countryRepository,IMapper mapper)
        { 
            _mapper = mapper;
            _CountryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable <Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCounties() 
        {
            var countries = _mapper.Map<List<CountryDto>>(_CountryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry(CountryDto country)
        {
            if (country == null) { return BadRequest(ModelState); }
            var countryMap=_mapper.Map<Country>(country);
            var countries = _CountryRepository.GetCountries()
                .Where(c=>c.name.ToLower().Trim()==country.name.ToLower().Trim())
                .FirstOrDefault();

            if (countries!=null)
            {
                ModelState.AddModelError("", "This country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_CountryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "error occured while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            if (!_CountryRepository.CountryExists(id))
            {
                return NotFound(); 
            }

            var country = _mapper.Map<CountryDto>(_CountryRepository.GetCountry(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpPut("{coId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int coId, [FromBody] CountryDto country)
        {
            if (country == null) return BadRequest(ModelState);
            if (coId != country.id) return BadRequest(ModelState);
            if (!_CountryRepository.CountryExists(country.id)) { return NotFound(); }

            var map = _mapper.Map<Country>(country);

            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (!_CountryRepository.UpdateCountry(map))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{CountryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int CountryId)
        {
            if (!_CountryRepository.CountryExists(CountryId)) { return NotFound(ModelState); }
            var CountryToDelete = _CountryRepository.GetCountry(CountryId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_CountryRepository.DeleteCountry(CountryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200,Type=typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId) 
        {
            var country=_mapper.Map<CountryDto>(_CountryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (country == null)
            return NotFound();
            return Ok(country);
        }
        
        [HttpGet("owner/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            if (!_CountryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var owners = _mapper.Map<List<OwnerDto>>(_CountryRepository.GetOwnersByCountry(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }
        
    }
}
