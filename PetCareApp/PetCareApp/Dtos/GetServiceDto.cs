using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [Range(0, 100000)]
        public float MinimumPrice { get; set; } = 0;
        [Range(0, 100000)]
        public float MaximumPrice { get; set; } = 0;
        public int RealTime { get; set; } = 0;
        public bool IsFixedTime { get; set; } = false;
        public bool IsNearestTime { get; set; } = false;
        public int MinimumTime { get; set; } = 0;
        public int MaximumTime { get; set; } = 0;
        public bool IsHidden { get; set; } = false;

        public string? AppUserId { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
