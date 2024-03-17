using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Data.Interfaces;
using Data.Models;

namespace GroceryStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult GetAllReviews()
        {
            List<Review> reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        public IActionResult GetReviewById(int reviewId)
        {
            Review review = _reviewService.GetReviewById(reviewId);
            if (review != null)
            {
                return Ok(review);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetReviewsByProductId/{productId}")]
        public IActionResult GetReviewsByProductId(int productId)
        {
            List<Review> reviews = _reviewService.GetReviewsByProductId(productId);
            return Ok(reviews);
        }

        [HttpGet]
        [Route("GetAverageRatingByProductId/{productId}")]
        public IActionResult GetAverageRatingByProductId(int productId)
        {
            double averageRating = _reviewService.GetAverageRatingByProductId(productId);
            return Ok(averageRating);
        }

         
    
        [HttpPost]
        [Route("AddReview")]
        public IActionResult AddReview([FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest();
            }

            // Perform any necessary validation before adding the review

            // Add the review
            Review addedReview = _reviewService.AddReview(review);

            return Ok(addedReview);
        }
    }
}
