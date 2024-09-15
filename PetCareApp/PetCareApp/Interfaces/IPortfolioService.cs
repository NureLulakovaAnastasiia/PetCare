using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IPortfolioService
    {
        public Task<string> AddPortfolio(List<AddPortfolioDto> potfolioDtos);
        public Task<string> UpdatePortfolio(List<PortfolioDto> portfolioDtos);
        public Task<string> RemovePortfolio(List<int> portfolioId);
        public List<PortfolioDto> GetMasterPortfolio(string masterId);
        //public string GetOrganizationPortfolio();
        //public string ChooseMasterPotfolio();
    }
}
