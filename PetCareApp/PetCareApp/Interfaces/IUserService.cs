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

        public Task<List<ReviewDto>> GetMasterReviews(string masterId);

        public Task<List<RecordInfoDto>> GetUserRecords();
        public Task<Result<bool>> CancelRecord(int recordId);
        public Task<Result<bool>> AddReview(ReviewDto review);
    }
}
