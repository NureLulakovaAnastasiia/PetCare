using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext applicationDBContext) : base(userManager, httpContextAccessor)
        {
            _dbContext = applicationDBContext;
            _mapper = mapper;
        }

        public async Task<Result<int>> AddPet(PetDto pet)
        {
            var res = new Result<int>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    res.ErrorMessage = "User not found";
                    return res;
                }
                var petToAdd = _mapper.Map<Pet>(pet);
                if (petToAdd != null)
                {
                    petToAdd.AppUserId = user.Id;
                    _dbContext.Add(petToAdd);
                    _dbContext.SaveChanges();
                    res.Data = petToAdd.Id;
                    return res;
                }
                res.ErrorMessage = "Error during creating";
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<string> UpdatePets(List<PetDto> pets)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var petsToUpdate = _mapper.Map<List<Pet>>(pets);
                if (petsToUpdate != null)
                {
                    foreach (var item in petsToUpdate)
                    {
                        item.AppUserId = user.Id;
                        _dbContext.Update(item);
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during updating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemovePet(int petId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var userPets = _dbContext.Pets.Where(x => x.AppUserId == user.Id);
                if (userPets != null)
                {
                    var item = userPets.First(x => x.Id == petId);
                    if (item != null && item.AppUserId == user.Id)
                    {
                        _dbContext.Pets.Remove(item);
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during deleting";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
