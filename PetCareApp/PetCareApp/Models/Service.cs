using Microsoft.EntityFrameworkCore;
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
        public float MaxmumPrice { get; set; } = 0;
        public int RealTime { get; set; }  = 0;
        public bool IsFixedTime { get; set; } = false;
        public bool IsNearestTime { get; set; } = false;
        public int MinimumTime { get; set; } = 0;
        public int MaxmumTime { get; set; } = 0;
        public bool IsHidden { get; set; } = false;

        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
    }
}
