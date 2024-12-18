namespace PetCareApp.Models
{
    public class FiltersModel
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Type { get; set; }
        public List<Tag>? Tags { get; set; }
        public double? Rate { get; set; }

        public FiltersModel() { }

    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
