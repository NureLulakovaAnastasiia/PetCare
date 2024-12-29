using NetTopologySuite.Geometries;
using PetCareApp.Dtos;
using PetCareApp.Models;
using Location = PetCareApp.Models.Location;

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

        public static Contacts MapFromDto(ContactsDto contacts)
        {
            var res = new Contacts
            {
                Id = contacts.Id,
                Address = contacts.Address,
                CityId = 0,
                AppUserId = contacts.AppUserId,
                Phone = contacts.Phone
            };
            res.Location = new Location
            {
                Id = contacts.Location == null ? 0 : contacts.Location.Id,
                Name = contacts.Location.Name,
                Coordinates = contacts.Location == null
                    ? null
                    : new NetTopologySuite.Geometries.Point(contacts.Location.Latitude, contacts.Location.Longitude) 
                    {
                        SRID = 4326 
                    },
                ContactsId = contacts.Id
            };

            return res;
        }
    }
}
