using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    //Послуга
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [Range(0, 100000)]
        public float MinimumPrice { get; set; } = 0;
        [Range(0, 100000)]
        public float MaximumPrice { get; set; } = 0;
        public int RealTime { get; set; }  = 0;
        public bool IsFixedTime { get; set; } = false;
        public bool IsNearestTime { get; set; } = false;
        public int MinimumTime { get; set; } = 0;
        public int MaximumTime { get; set; } = 0;
        public bool IsHidden { get; set; } = false;

        public double Rate { get
            {
                var rates = Reviews.Select(x => x.Rate).ToList();
                if (rates.Any())
                {
                    return rates.Average();
                }
                return 0;
            } }

        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Record>? Records { get; set; } = new List<Record>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Tag>? Tags { get; set; }

        public List<ServiceLimitation> ServiceLimitations { get; set; } = new List<ServiceLimitation>();
    }
}
