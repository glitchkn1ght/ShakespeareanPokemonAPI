//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Services
{
    using ShakespeareanPokemonAPI.Models;
    using ShakespeareanPokemonAPI.Models.PokeAPI;
    using System.Threading.Tasks;

    public interface IPokeApiService
    {
        public Task<FlavourTextEntry> GetPokemonDescription(string pokemonName);
    }

    public class PokeApiService : IPokeApiService
    {
        public Task<FlavourTextEntry> GetPokemonDescription(string pokemonName)
        {
            return Task.FromResult(new FlavourTextEntry());
        
        }
    }
}
