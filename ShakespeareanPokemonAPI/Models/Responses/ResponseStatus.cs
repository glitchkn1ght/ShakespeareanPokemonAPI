//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.Responses
{
    public class ResponseStatus
    {
        public ResponseStatus() { }

        public ResponseStatus(bool isSuccess, int statusCode, string statusMessage)
        {
            this.IsSuccess = isSuccess;
            this.StatusCode = statusCode;
            this.StatusMessage= statusMessage;
        }
        
        public bool IsSuccess { get; set; } = false;

        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
