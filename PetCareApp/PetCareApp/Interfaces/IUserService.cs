using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface IUserService
    {
        public Task<Result<int>> AddPet(PetDto pet);
        public Task<string> UpdatePets(List<PetDto> pets);
        public Task<string> RemovePet(int petId);
        public MasterDto GetMasterData(string masterId);

        public Task<Result<List<ReviewDto>>> GetMasterReviews(string masterId);

        public Task<List<RecordInfoDto>> GetUserRecords();
        public Task<Result<bool>> CancelRecord(int recordId);
        public Task<Result<bool>> AddReview(ReviewDto review);
        public Task<Result<bool>> RemoveReview(int reviewId);
        public Task<Result<bool>> RemoveReviewComment(int commentId);
        public Task<Result<bool>> AddReviewComment(ReviewCommentDto reviewComment);
    }
}
