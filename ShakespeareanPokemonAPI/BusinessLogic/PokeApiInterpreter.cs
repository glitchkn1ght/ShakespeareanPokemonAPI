//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IPokeApiInterpreter
    {
        public Task<PokeResponse> InterepretPokeApiResponse(HttpResponseMessage pokeApiResponse, string descriptionLanguage);
    }

    public class PokeApiInterpreter : IPokeApiInterpreter
    {
        IPokemonDescriptionMapper DescriptionMapper;

        public PokeApiInterpreter(IPokemonDescriptionMapper descriptionMapper)
        {
            this.DescriptionMapper = descriptionMapper;
        }

        public async Task<PokeResponse> InterepretPokeApiResponse(HttpResponseMessage ApiResponse, string descriptionLanguage)
        {
            PokeResponse pokemonResponse = new PokeResponse();

            if (ApiResponse.IsSuccessStatusCode)
            {
                pokemonResponse = await this.DescriptionMapper.MapPokemonDescription(ApiResponse, descriptionLanguage);

                if (!string.IsNullOrWhiteSpace(pokemonResponse.PokemonDescription))
                {
                    pokemonResponse.IsSuccess = true;
                }

                else
                {
                    pokemonResponse.ErrorCode = 404;
                    pokemonResponse.ErrorMessage = "Pokemon was found on Api but no valid description could be mapped";
                }
            }

            else
            {
                //todo
            }

            return pokemonResponse;
        }
    }
}
