namespace PetCareApp.Dtos
{
    public class OrganizationDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AppUserId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public GetContactsDto Contacts { get; set; }

    }
}
