using NetTopologySuite.Geometries;

namespace PetCareApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";
        public Point Coordinates { get; set; }
        public int ContactsId {  get; set; }

        public Contacts? Contacts { get; set; }
    }
}
