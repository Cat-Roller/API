using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pokemon_Review_App_Web_API_.Dto;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;
using Pokemon_Review_App_Web_API_.Repository;

namespace Pokemon_Review_App_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createPokemon([FromQuery] int ownerId,[FromQuery]int categoryId, [FromBody] PokemonDto pokemonRaw)
        {
            if (pokemonRaw==null)
            {
                return BadRequest(ModelState);
            }
            var pokemons = _pokemonRepository.GetPokemons()
                .Where(p=>p.name.Trim().ToUpper()==pokemonRaw.name.Trim().ToUpper())
                .FirstOrDefault();

            if (pokemons!=null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422,ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemon = _mapper.Map<Pokemon>(pokemonRaw);

            if (!_pokemonRepository.createPokemon(ownerId,categoryId,pokemon))
            {
                ModelState.AddModelError("", "Was unable to create");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner([FromBody] PokemonDto pokemon, int pokeId)
        {
            if (pokemon == null) { return BadRequest(ModelState); }

            if (pokemon.id != pokeId) { return BadRequest(ModelState); }

            if (!_pokemonRepository.PokemonExists(pokeId)) { return NotFound(); }

            var map = _mapper.Map<Pokemon>(pokemon);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!_pokemonRepository.UpdatePokemon(map))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{PokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int PokeId)
        {
            if (!_pokemonRepository.PokemonExists(PokeId)) { return NotFound(ModelState); }
            var PokemonToDelete = _pokemonRepository.GetPokemon(PokeId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_pokemonRepository.DeletePokemon(PokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            decimal rating = _pokemonRepository.GetPokemonRating(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }

    }
}
