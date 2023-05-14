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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonRepository _pokeRepository;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository,IMapper mapper,IPokemonRepository pokemonRepository,ICountryRepository countryRepository)
        {
            _ownerRepository=ownerRepository;
            _mapper=mapper;
            _pokeRepository = pokemonRepository;
            _countryRepo=countryRepository;
        }
        

        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(ICollection<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners() 
        { 
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var owners = _ownerRepository.GetOwners()
                .Where(c => c.name.Trim().ToUpper() == ownerCreate.name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.country = _countryRepo.GetCountry(countryId);

            if (!_ownerRepository.createOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
            {
                return NotFound(); 
            }

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(id));

            if (!ModelState.IsValid) { return BadRequest();}

            return Ok(owner);
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner([FromBody] OwnerDto owner,int ownerId)
        {
            if (owner == null) { return BadRequest(ModelState); }

            if(owner.id!= ownerId) { return BadRequest(ModelState); }

            if (!_ownerRepository.OwnerExists(ownerId)) { return NotFound(); }

            var map = _mapper.Map<Owner>(owner);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!_ownerRepository.UpdateOwner(map))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{OwnerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int OwnerId)
        {
            if (!_ownerRepository.OwnerExists(OwnerId)) { return NotFound(ModelState); }
            var OwnerToDelete = _ownerRepository.GetOwner(OwnerId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_ownerRepository.DeleteOwner(OwnerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByOwner(int ownerId) 
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid) { return BadRequest(); }
            return Ok(pokemons);
        }
        
        [HttpGet("pokemon/{id}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByPokemon(int id)
        {
            if(!_pokeRepository.PokemonExists(id))return NotFound();
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersByPokemon(id));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(owners);
        }
        
    }
}
