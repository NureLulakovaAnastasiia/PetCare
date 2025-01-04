using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface IUserService
    {
        public Task<Result<int>> AddPet(PetDto pet);
        public Task<string> UpdatePets(List<PetDto> pets);
        public Task<string> RemovePet(int petId);
    }
}
