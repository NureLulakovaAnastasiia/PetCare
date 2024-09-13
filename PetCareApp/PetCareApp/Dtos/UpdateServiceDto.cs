using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class UpdateServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [Range(0, 100000)]
        public float MinimumPrice { get; set; }
        [Range(0, 100000)]
        public float MaximumPrice { get; set; } 
        public int RealTime { get; set; } 
        public bool IsFixedTime { get; set; }
        public bool IsNearestTime { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public bool IsHidden { get; set; } 
        public string? AppUserId { get; set; }
    }
}
