using PetCareApp.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetCareApp.Models
{
    public class Country
    {
        [Key]
        [JsonPropertyName("countryId")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int Id { get; set; }
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
        [JsonPropertyName("countryName")]
        public string LocalizedName { get; set; } // JSON string for localized names
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

         
        public ICollection<City>? Cities { get; set; }
    }

    public class City
    {
        [Key]
        [JsonPropertyName("geonameId")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int Id { get; set; }
        [JsonPropertyName("countryId")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int CountryId { get; set; }
        [JsonPropertyName("name")]
        public string LocalizedName { get; set; }
        [JsonPropertyName("adminName1")]
        public string AdminRegion { get; set; }
        [JsonPropertyName("lat")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Latitude { get; set; }
        [JsonPropertyName("lng")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Country? Country { get; set; }
    }

}
