//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; } = false;

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
