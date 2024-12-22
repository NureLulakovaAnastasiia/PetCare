namespace PetCareApp.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ContactsId { get; set; }
    }
}
