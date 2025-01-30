using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class OrganizationPorfolio
    {
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public int PortfolioId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Portfolio? Portfolio { get; set; }
    }
}
