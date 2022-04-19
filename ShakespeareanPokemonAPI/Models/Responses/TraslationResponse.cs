//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.Responses
{
    using Newtonsoft.Json;
    using ShakespeareanPokemonAPI.Models.Responses;

    public class TraslationResponse : BaseResponse
    {
        public string TranslatedText { get; set; }
    }
}
