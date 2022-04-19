//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.PokeAPI
{
    using Newtonsoft.Json;
    using ShakespeareanPokemonAPI.Models.PokeAPI;

    public class FlavourTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavourText { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }

        [JsonProperty("version")]
        public Version version { get; set; }
    }
}
