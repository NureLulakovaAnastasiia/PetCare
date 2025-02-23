using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class AddServiceDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Minimum price is required.")]
        [Range(0, 100000, ErrorMessage = "Minimum price must be between 0 and 100,000.")]
        public float MinimumPrice { get; set; }

        [Required(ErrorMessage = "Maximum price is required.")]
        [Range(1, 100000, ErrorMessage = "Maximum price must be between 0 and 100,000.")]
        public float MaximumPrice { get; set; }

        [Required(ErrorMessage = "Real time is required.")]
        [Range(1, 1000, ErrorMessage = "Real time must be between 1 and 1000 minutes.")]
        public int RealTime { get; set; }

        [Required(ErrorMessage = "Minimum time is required.")]
        [Range(1, 1000, ErrorMessage = "Minimum time must be between 1 and 1000 minutes.")]
        public int MinimumTime { get; set; }

        [Required(ErrorMessage = "Maximum time is required.")]
        [Range(1, 1000, ErrorMessage = "Maximum time must be between 1 and 1000 minutes.")]
        public int MaximumTime { get; set; }
        [Required]
        public bool IsFixedTime { get; set; }
        [Required]
        public bool IsNearestTime { get; set; } = false;
        [Required]
        
        public bool IsHidden { get; set; } = false;
    }
}
