//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.FunTranslationsApi
{
    using Newtonsoft.Json;

    public class Error
    {
        public Error()
        {
            this.ErrorDetail = new ErrorDetail();
        }
        
        [JsonProperty("error")]
        public ErrorDetail ErrorDetail { get; set; }
    }
}
