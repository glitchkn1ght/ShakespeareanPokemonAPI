//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Models.Responses
{
    public class ShakespeareanPokemonResponse : BaseResponse
    {
        public ShakespeareanPokemonResponse()
        {
            this.Pokemon = new ShakespeareanPokemon();
        }

        public ShakespeareanPokemon Pokemon { get; set; }
    }
}
