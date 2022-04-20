//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.FunTranslationsApi
{
    using Newtonsoft.Json;

    public class ErrorDetail
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
