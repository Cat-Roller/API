using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        ICollection<Review> ReviewsByReviewer(int id);
        bool ReviewerExists(int id);

        bool Save();
        bool CreateReviewer(Reviewer reviewer);

        bool UpdateReviewer(Reviewer reviewer);

        bool DeleteReviewer(Reviewer reviewer);
    }
}
