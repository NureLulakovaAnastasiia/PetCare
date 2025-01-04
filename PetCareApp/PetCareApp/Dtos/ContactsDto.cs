using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class ContactsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Phone { get; set; }

        public string AppUserId { get; set; } = "";

        public LocationDto? Location { get; set; }


        public ContactsDto() { }
        public ContactsDto(ContactsDto contacts)
        {
            this.Id = contacts.Id;
            this.Address = contacts.Address;
            this.City = contacts.City;
            this.Phone = contacts.Phone;
            this.AppUserId = contacts.AppUserId;
            this.Location = new LocationDto();
            if(contacts.Location != null)
            {
                this.Location.Id = contacts.Location.Id;
                this.Location.ContactsId = contacts.Id;
                this.Location.Latitude = contacts.Location.Latitude;
                this.Location.Longitude = contacts.Location.Longitude;
                this.Location.Name = contacts.Location.Name;
            }
        }
    }
}
