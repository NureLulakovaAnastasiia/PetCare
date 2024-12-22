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
        public double Rate { get; set; } = 0;
        public string? AppUserId { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();

        public GetServiceDto() { }
        public GetServiceDto(GetServiceDto dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.Description = dto.Description;
            this.MinimumPrice = dto.MinimumPrice;
            this.MaximumPrice = dto.MaximumPrice;
            this.RealTime = dto.RealTime;
            this.IsHidden = dto.IsHidden;
            this.IsFixedTime = dto.IsFixedTime;
            this.IsNearestTime = dto.IsNearestTime;
            this.MinimumTime = dto.MinimumTime;
            this.MaximumTime = dto.MaximumTime;
        }
    }
}
