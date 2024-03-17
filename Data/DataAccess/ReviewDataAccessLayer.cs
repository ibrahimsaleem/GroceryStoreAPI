
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data.DataAccess
{
    public class ReviewDataAccessLayer : IReviewService
    {
        private readonly GroceryDBContext _dbContext;

        public ReviewDataAccessLayer(GroceryDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Review> GetAllReviews()
        {
            return _dbContext.Review.ToList();
        }

        public Review GetReviewById(int reviewId)
        {
            return _dbContext.Review.FirstOrDefault(r => r.ReviewId == reviewId);
        }

        public List<Review> GetReviewsByProductId(int productId)
        {
            return _dbContext.Review.Where(r => r.ProductId == productId).ToList();
        }

        public double GetAverageRatingByProductId(int productId)
        {
            return _dbContext.Review
                .Where(r => r.ProductId == productId)
                .Average(r => r.Rating);
        }

        public Review AddReview(Review review)
        {
            try
            {
                _dbContext.Review.Add(review);
                _dbContext.SaveChanges();

                return review;
            }
            catch
            {
                throw;
            }
        }
    }
}

