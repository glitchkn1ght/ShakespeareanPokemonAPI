//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IPokeApiInterpreter
    {
        public Task<PokeResponse> InterepretPokeApiResponse(HttpResponseMessage pokeApiResponse, string descriptionLanguage);
    }

    public class PokeApiInterpreter : IPokeApiInterpreter
    {
        private readonly ILogger<PokeApiInterpreter> Logger;
        IPokemonDescriptionMapper DescriptionMapper;

        public PokeApiInterpreter(ILogger<PokeApiInterpreter> logger, IPokemonDescriptionMapper descriptionMapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.DescriptionMapper = descriptionMapper ?? throw new ArgumentNullException(nameof(descriptionMapper));
        }

        public async Task<PokeResponse> InterepretPokeApiResponse(HttpResponseMessage ApiResponse, string descriptionLanguage)
        {
            PokeResponse pokemonResponse = new PokeResponse();

            if (ApiResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=InterepretPokeApiResponse], Status=Success, Message=Success code received from PokeApi endpoint, mapping description.");

                pokemonResponse = await this.DescriptionMapper.MapPokemonDescription(ApiResponse, descriptionLanguage);

                if (!string.IsNullOrWhiteSpace(pokemonResponse.PokemonDescription))
                {
                    this.Logger.LogInformation($"[Operation=InterepretPokeApiResponse], Status=Success, Message=Successfully mapped description from response");

                    pokemonResponse.IsSuccess = true;
                }

                else
                {
                    this.Logger.LogWarning($"[Operation=InterepretPokeApiResponse], Status=Success, Message=Could not map description from response");

                    pokemonResponse.ErrorCode = 404;
                    pokemonResponse.ErrorMessage = "Pokemon was found on Api but no valid description could be mapped";
                }
            }

            else
            {
                this.Logger.LogWarning($"[Operation=InterepretPokeApiResponse], Status=Failure, Message=Failure code received from PokeApi endpoint, mapping error");

                //todo
            }

            return pokemonResponse;
        }
    }
}
