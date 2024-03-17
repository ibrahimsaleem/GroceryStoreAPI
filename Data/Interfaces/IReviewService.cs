using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IReviewService
    {
        List<Review> GetAllReviews();
        Review GetReviewById(int reviewId);
        List<Review> GetReviewsByProductId(int productId);
        double GetAverageRatingByProductId(int productId);
        Review AddReview(Review review);
    }
}
