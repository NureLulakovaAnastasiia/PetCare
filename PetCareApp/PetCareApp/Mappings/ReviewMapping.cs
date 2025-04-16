using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Mappings
{
    public class ReviewMapping
    {
        public static ReviewDto MapReview(Review review)
        {
            var res = new ReviewDto
            {
                Id = review.Id,
                Text = review.Text,
                Photo = review.Photo,
                Rate = review.Rate,
                AppUserId = review.AppUserId,
                AppUserName = review.AppUser != null ? review.AppUser.FirstName + " " + review.AppUser.LastName : "",
                ServiceId = review.ServiceId,
                ServiceName  = review.Service != null ? review.Service.Name : ""
            };

            res.Comments = MapReviewComments(review.Comments);
            return res;
        }

        public static List<ReviewCommentDto> MapReviewComments(List<ReviewComment> list)
        {
            var res = new List<ReviewCommentDto>();
            foreach (var comment in list)
            {
                res.Add(new ReviewCommentDto
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    AppUserId = comment.AppUserId,
                    AppUserName = comment.AppUser != null ? comment.AppUser.FirstName + " " + comment.AppUser.LastName : "",
                    ReviewId = comment.ReviewId
                });
            }
            return res;
        }
    }
}
