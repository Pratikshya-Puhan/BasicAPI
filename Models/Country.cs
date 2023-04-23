using System.Text.Json.Serialization;

namespace BasicAPI.Models
{
    public abstract class BaseCountry
    {
        [JsonPropertyOrder(1)]
        public string? Name { get; set; }
        
        [JsonPropertyOrder(2)]
        public string? ShortCode { get; set; }

        [JsonPropertyOrder(3)]
        public string? ISDCode { get; set; }
    }

    public class Country: BaseCountry
    {
        [JsonPropertyOrder(0)]
        public int Id { get; set; }

        [JsonPropertyOrder(4)]
        public string? FlagUrl { get; set; }
    }

    public class SingleCountry : BaseCountry
    {
        [JsonPropertyOrder(0)]
        public int Id { get; set; }

        [JsonPropertyOrder(4)]
        public string? FlagUrl { get; set; }

        [JsonPropertyOrder(5)]
        public List<State>? States { get; set; }
    }

    public class CreateCountryDTO: BaseCountry
    {
        [JsonPropertyOrder(4)]
        public IFormFile? File { get; set; }
    }
}
