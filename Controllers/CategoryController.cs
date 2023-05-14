using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_App_Web_API_.Dto;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;
using System.Collections;

namespace Pokemon_Review_App_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository repository,IMapper mapper)
        {
            _CategoryRepository = repository;
            _mapper=mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var categories = Ok(_mapper.Map<List<CategoryDto>>(_CategoryRepository.GetCategories().ToList()));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return categories;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody]CategoryDto categoryRaw)
        {
            if (categoryRaw == null) { return BadRequest(ModelState);}
            var categoryMap = _mapper.Map<Category>(categoryRaw);

            var categories = _CategoryRepository.GetCategories()
                .Where(c=>c.name.ToLower().Trim()==categoryRaw.name.ToLower().Trim())
                .FirstOrDefault();

            if (categories!=null)
            {
                ModelState.AddModelError("", "This category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_CategoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("","something went wrong while saving");
                return StatusCode(500,ModelState);
            }

            return Ok("Success");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type =typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            var category=_mapper.Map<CategoryDto>(_CategoryRepository.GetCategory(id));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPut("{CatId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCategory(int CatId, [FromBody] CategoryDto category)
        {
            if (category == null) { return BadRequest(ModelState); }
            if (category.id != CatId)
            {
                return BadRequest(ModelState);
            }

            if (!_CategoryRepository.CategoryExists(CatId))
            {
                return NotFound();
            }

            var categorymap = _mapper.Map<Category>(category);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_CategoryRepository.UpdateCategory(categorymap))
            {
                ModelState.AddModelError("", "somethign went wrong while updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{CatId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int CatId) 
        {
            if (!_CategoryRepository.CategoryExists(CatId)) { return NotFound(ModelState); }
            var categoryToDelete = _CategoryRepository.GetCategory(CatId);
            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_CategoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("pokemon/{id}")]
        [ProducesResponseType(200, Type =typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByCategory(int id)
        {
            if (!_CategoryRepository.CategoryExists(id))
            {
                return NotFound();
            }

            var pokemons=_mapper.Map<List<PokemonDto>>(_CategoryRepository.GetPokemonsByCategory(id));

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(pokemons);
        }
    }
}
