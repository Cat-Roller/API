using Pokemon_Review_App_Web_API_.Data;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private DataContext _context;

        public ReviewRepository(DataContext dataContext)
        {
            _context=dataContext;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(r=>r.id==id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsByPokemon(int id)
        {
            return _context.Reviews.Where(r=>r.pokemon.id==id).ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r=>r.id==id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0?true:false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }
    }
}
