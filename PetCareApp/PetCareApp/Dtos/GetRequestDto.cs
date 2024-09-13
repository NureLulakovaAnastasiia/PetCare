using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetRequestDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; } 
        public string AppUserId { get; set; }
    }
}
