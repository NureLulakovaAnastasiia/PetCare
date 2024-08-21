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
        public DbSet<Pet> Breaks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<RecordCancel> RecordCancels { get; set; }

    }
}
