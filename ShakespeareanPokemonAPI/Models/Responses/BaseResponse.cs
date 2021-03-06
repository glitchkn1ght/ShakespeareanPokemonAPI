//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.Responses
{
    public abstract class BaseResponse
    {
        public BaseResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
