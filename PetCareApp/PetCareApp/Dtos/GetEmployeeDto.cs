namespace PetCareApp.Dtos
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string masterId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int OrganizationId {  get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public DateTime AcceptanceDate { get; set; }
        public DateTime? DismissalDate { get; set; }

    }
}
