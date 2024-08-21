using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Models;

namespace PetCareApp.Data
{
    public class ApplicationDBContext: IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Service> Services { get; set; }

    }
}
