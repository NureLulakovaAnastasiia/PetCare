namespace PetCareApp.Models
{
    public class FiltersModel
    {
        public int? City { get; set; }
        public int? Country { get; set; }
        public string? Type { get; set; }
        public List<Tag>? Tags { get; set; }
        public double? Rate { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public string? Name { get; set; }

        public string Localization { get; set; } = "en";
        public FiltersModel() { }

    }
}
