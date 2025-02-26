namespace PetCareApp.Dtos
{
    public class OrganizationDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AppUserId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Photo { get; set; }

        public bool IsInOrg { get; set; } = false;

        public GetContactsDto Contacts { get; set; }

    }
}
