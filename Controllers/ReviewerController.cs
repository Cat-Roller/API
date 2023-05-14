using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_App_Web_API_.Dto;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;
using Pokemon_Review_App_Web_API_.Repository;

namespace Pokemon_Review_App_Web_API_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepo;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository,IMapper mapper)
        {
            _reviewerRepo=reviewerRepository;
            _mapper=mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type=typeof(ICollection<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers= _mapper.Map<List<ReviewerDto>>(_reviewerRepo.GetReviewers());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(reviewers);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody]ReviewerDto reviewer)
        {
            if (!ModelState.IsValid) {return BadRequest(ModelState);}
            var reviewerMap = _mapper.Map<Reviewer>(reviewer);

            var reviewers = _reviewerRepo.GetReviewers().Where(r => (r.lastName + r.firstName).ToLower().Trim()
            == (reviewer.lastName + reviewer.firstName).ToLower().Trim()).FirstOrDefault();

            if (reviewers!=null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_reviewerRepo.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "something went wromg while saving");
                return StatusCode(500,ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int id)
        {
            if (!_reviewerRepo.ReviewerExists(id))
            {
                return NotFound();
            }

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepo.GetReviewer(id));

            if (!ModelState.IsValid) { return BadRequest(); }

            return Ok(reviewer);
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner([FromBody] ReviewerDto reviewer, int reviewerId)
        {
            if (reviewer == null) { return BadRequest(ModelState); }

            if (reviewer.id != reviewerId) { return BadRequest(ModelState); }

            if (!_reviewerRepo.ReviewerExists(reviewerId)) { return NotFound(); }

            var map = _mapper.Map<Reviewer>(reviewer);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!_reviewerRepo.UpdateReviewer(map))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ReviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int ReviewerId)
        {
            if (!_reviewerRepo.ReviewerExists(ReviewerId)) { return NotFound(ModelState); }
            var ReviewerToDelete = _reviewerRepo.GetReviewer(ReviewerId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_reviewerRepo.DeleteReviewer(ReviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("{id}/reviews")]
        [ProducesResponseType(200,Type= typeof(ICollection<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int id)
        {
            if (!_reviewerRepo.ReviewerExists(id)) { return NotFound(); }

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepo.ReviewsByReviewer(id));
            if (!ModelState.IsValid) { return NotFound(); }
            return Ok(reviews);
        }
    }   
}
