using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsByPokemon(int  id);
        bool ReviewExists(int id);

        bool Save();
        bool CreateReview(Review review);

        bool UpdateReview(Review review);

        bool DeleteReview(Review review);
    }
}
