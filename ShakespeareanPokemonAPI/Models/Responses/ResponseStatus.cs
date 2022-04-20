//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.Responses
{
    public class ResponseStatus
    {
        public bool IsSuccess { get; set; } = false;

        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
