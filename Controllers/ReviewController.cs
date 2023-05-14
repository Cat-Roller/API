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
    public class ReviewController : Controller
    {
        private readonly IPokemonRepository _pokemonRepo;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _rerRepository;

        public ReviewController(IReviewRepository reviewRepository,IPokemonRepository pr,IMapper mapper,IReviewerRepository rr)
        {
            _pokemonRepo = pr;
            _reviewRepository=reviewRepository;
            _mapper=mapper;
            _rerRepository=rr;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(ICollection<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery]int pokeId, [FromQuery]int reviewerId, [FromBody]ReviewDto reviewRaw)
        { 
            if (reviewRaw == null) { return BadRequest(ModelState); }

            var reviews = _reviewRepository.GetReviews()
                .Where(review => review.title.Trim().ToUpper().Equals(reviewRaw.title.Trim().ToUpper()))
                .FirstOrDefault();

            if (reviews!=null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            var review = _mapper.Map<Review>(reviewRaw);

            review.reviewer = _rerRepository.GetReviewer(reviewerId);
            review.pokemon = _pokemonRepo.GetPokemon(pokeId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.CreateReview(review))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500,ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type=typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner([FromBody] ReviewDto review, int reviewId)
        {
            if (review == null) { return BadRequest(ModelState); }

            if (review.id != reviewId) { return BadRequest(ModelState); }

            if (!_reviewRepository.ReviewExists(reviewId)) { return NotFound(); }

            var map = _mapper.Map<Review>(review);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!_reviewRepository.UpdateReview(map))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ReviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int ReviewId)
        {
            if (!_reviewRepository.ReviewExists(ReviewId)) { return NotFound(ModelState); }
            var reviewToDelete = _reviewRepository.GetReview(ReviewId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type=typeof(ICollection<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByPokemon(int pokeId)
        {
            if (!_pokemonRepo.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsByPokemon(pokeId));
            if (!ModelState.IsValid) { return BadRequest(ModelState);}
            return Ok(reviews);
        }
    }
}
