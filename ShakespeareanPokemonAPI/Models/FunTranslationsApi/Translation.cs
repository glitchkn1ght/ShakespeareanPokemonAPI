//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.FunTranslationsApi
{
    using Newtonsoft.Json;
    
    public class Translation
    {
        [JsonProperty("success")]
        public TranslationSuccess TranslationSuccess { get; set; }

        [JsonProperty("contents")]
        public TranslationContents TranslationContents { get; set; }
    }
}
