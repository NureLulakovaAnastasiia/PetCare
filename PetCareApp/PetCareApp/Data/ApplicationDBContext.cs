using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Models;
using System.Reflection.Emit;


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
        public DbSet<RequestToOrganization> RequestsToOrganization { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=CompNastia\\SQLEXPRESS;Initial Catalog=PetCare;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                x => x.UseNetTopologySuite() // Enable NetTopologySuite
            );
        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Location>(entity =>
        //    {
        //        entity.Property(e => e.Coordinates)
        //              .HasColumnType("geography");
        //    });
        //}

            //builder.Entity<Country>(entity =>
            //{
            //    entity.ToTable("Countries");
            //    entity.Property(e => e.LocalizedName)
            //          .HasColumnType("NVARCHAR(MAX)");
            //});

            //builder.Entity<City>(entity =>
            //{
            //    entity.ToTable("Cities");
            //    entity.Property(e => e.LocalizedName)
            //          .HasColumnType("NVARCHAR(MAX)");
            //});

            //    base.OnModelCreating(builder);
            //    List<IdentityRole> roles = new List<IdentityRole>
            //        {
            //        new IdentityRole
            //        {
            //            Name = "Admin",
            //            NormalizedName = "ADMIN"
            //        },
            //        new IdentityRole
            //        {
            //            Name = "User",
            //            NormalizedName = "USER"
            //        },
            //        new IdentityRole
            //        {
            //            Name = "Master",
            //            NormalizedName = "MASTER"
            //        },
            //        new IdentityRole
            //        {
            //            Name = "Organization",
            //            NormalizedName = "ORGANIZATION"
            //        }
            //    };

            //    builder.Entity<IdentityRole>().HasData(roles);
            //}


        }
}
