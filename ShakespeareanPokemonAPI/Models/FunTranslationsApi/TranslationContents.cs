//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.FunTranslationsApi
{
    using Newtonsoft.Json;
    public class TranslationContents
    {
        [JsonProperty("translated")]
        public string TranslatedText { get; set; }

        [JsonProperty("text")]
        public string OriginalText { get; set; }

        [JsonProperty("translation")]
        public string TranlastionType { get; set; }
    }
}
