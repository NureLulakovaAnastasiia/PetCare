using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Mappings
{
    public class ContactsMapping
    {
        public static ContactsDto MapContact(Contacts contacts)
        {
            var res = new ContactsDto
            {
                Id = contacts.Id,
                Address = contacts.Address,
                City = contacts.CityId.ToString(),
                AppUserId = contacts.AppUserId,
                Phone = contacts.Phone
            };

            if (contacts.Location != null)
            {
                res.Location = new LocationDto
                {
                    Id = contacts.Location.Id,
                    Name = contacts.Location.Name,
                    Longitude = contacts.Location.Coordinates.Y,
                    Latitude = contacts.Location.Coordinates.X,
                    ContactsId = contacts.Id
                };
            }
            return res;
        }
    }
}
