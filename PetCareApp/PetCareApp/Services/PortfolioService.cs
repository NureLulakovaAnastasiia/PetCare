using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public class PortfolioService : BaseService, IPortfolioService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public PortfolioService(UserManager<AppUser> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext applicationDBContext) : base(userManager, httpContextAccessor)
        {
            _dbContext = applicationDBContext;
            _mapper = mapper;
        }

        public async Task<string> AddPortfolio(List<AddPortfolioDto> portfolioDtos)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var portfolio = _mapper.Map<List<Portfolio>>(portfolioDtos);
                if (portfolio != null)
                {
                    foreach (var item in portfolio)
                    {
                        item.AppUserId = user.Id;
                        _dbContext.Add(item);
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during creating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public  List<PortfolioDto> GetMasterPortfolio(string masterId)
        {
            var res = new List<PortfolioDto>();
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == masterId);
                if (user != null)
                {
                    var portfolio = _dbContext.Portfolios.Where(x => x.AppUserId == user.Id).ToList();
                    if (portfolio != null)
                    {
                        res = _mapper.Map<List<PortfolioDto>>(portfolio);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }

        public async Task<string> RemovePortfolio(List<int> portfolioIds)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var portfolio = _dbContext.Portfolios.Where(x => x.AppUserId == user.Id);
                if (portfolio != null)
                {
                    foreach (var id in portfolioIds)
                    {
                        var item = portfolio.First(x => x.Id == id);
                        if (item != null && item.AppUserId == user.Id)
                        {
                            _dbContext.Portfolios.Remove(item);
                        }
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

        public async Task<string> UpdatePortfolio(List<PortfolioDto> portfolioDtos)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }
                var portfolio = _mapper.Map<List<Portfolio>>(portfolioDtos);
                if (portfolio != null)
                {
                   foreach(var item in portfolio)
                    {
                        item.AppUserId = user.Id;
                        _dbContext.Update(item);
                    }
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during creating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
