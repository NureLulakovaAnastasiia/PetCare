using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class AddServiceDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; } 
        [Range(0, 100000)]
        [Required]
        public float MinimumPrice { get; set; }
        [Range(0, 100000)]
        [Required]
        public float MaximumPrice { get; set; }
        [Required]
        public int RealTime { get; set; }
        [Required]
        public bool IsFixedTime { get; set; }
        [Required]
        public bool IsNearestTime { get; set; } = false;
        [Required]
        public int MinimumTime { get; set; }
        [Required]
        public int MaximumTime { get; set; }
        public bool IsHidden { get; set; } = false;
    }
}
