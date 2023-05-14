using Microsoft.EntityFrameworkCore;
using Pokemon_Review_App_Web_API_.Data;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private DataContext _dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dataContext.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _dataContext.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int id)
        {
            return _dataContext.Reviewers.Where(r => r.id == id).Include(e=>e.reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _dataContext.Reviewers.ToList();
        }

        public bool ReviewerExists(int id)
        {
            return _dataContext.Reviewers.Any(r => r.id == id);
        }

        public ICollection<Review> ReviewsByReviewer(int id)
        {
            return _dataContext.Reviews.Where(r=>r.reviewer.id == id).ToList();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved>0? true: false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _dataContext.Update(reviewer);
            return Save();
        }
    }
}
