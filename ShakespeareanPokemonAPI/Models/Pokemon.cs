//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models
{
    using Newtonsoft.Json;

    public class Pokemon
    {
        [JsonProperty("name")]
        string Name { get; set; }

        [JsonProperty("description")]
        string Description { get; set; }
    }
}
