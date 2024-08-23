using Microsoft.AspNetCore.Identity;
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
        public DbSet<PetCareApp.Models.Service> Services { get; set; }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<RecordCancel> RecordCancels { get; set; }
        public DbSet<ServiceLimitation> ServiceLimitations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewComment> ReviewComments { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationEmployee> OrganizationEmployees { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole>
                {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Name = "Master",
                    NormalizedName = "MASTER"
                },
                new IdentityRole
                {
                    Name = "Organization",
                    NormalizedName = "ORGANIZATION"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

      
    }
}
