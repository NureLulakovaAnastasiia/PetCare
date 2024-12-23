using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareApp.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

       
        public List<Service>? Services { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Tag tag && Id == tag.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
