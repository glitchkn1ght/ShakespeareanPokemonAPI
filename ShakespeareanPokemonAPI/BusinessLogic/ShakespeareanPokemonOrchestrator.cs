//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.Models.Responses;
    using ShakespeareanPokemonAPI.Services;
    using System;
    using System.Threading.Tasks;

    public interface IShakespeareanPokemonOrchestrator
    {
        public Task<ShakespeareanPokemonResponse> GetShakespeareanPokemon(string PokemonName);
    }

    public class ShakespeareanPokemonOrchestrator : IShakespeareanPokemonOrchestrator
    {
        private readonly ILogger<ShakespeareanPokemonOrchestrator> Logger;
        private readonly IFunTranslationsApiService FTApiService;
        private readonly IPokeApiService PokeApiService;

        public ShakespeareanPokemonOrchestrator
        (
            ILogger<ShakespeareanPokemonOrchestrator> logger, 
            IFunTranslationsApiService ftApiService,
            IPokeApiService pokeApiService
        )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.FTApiService = ftApiService ?? throw new ArgumentNullException(nameof(ftApiService));
            this.PokeApiService = pokeApiService ?? throw new ArgumentNullException(nameof(pokeApiService));
        }

        public async Task<ShakespeareanPokemonResponse> GetShakespeareanPokemon(string pokemonName)
        {
            ShakespeareanPokemonResponse response = new ShakespeareanPokemonResponse();

            PokeApiResponse pokeApiResponse = await this.PokeApiService.GetPokemonFromApi(pokemonName);

            if (!pokeApiResponse.IsSuccess)
            {
                response.IsSuccess = false;
                response.StatusCode = pokeApiResponse.StatusCode;
                response.StatusMessage = pokeApiResponse.StatusMessage;
                
                return response;
            }

            TraslationResponse traslationResponse = await this.FTApiService.TranslatePokemonDescription(pokeApiResponse.PokemonDescription);

            if(!traslationResponse.IsSuccess)
            {
                response.IsSuccess = false;
                response.StatusCode = traslationResponse.StatusCode;
                response.StatusMessage = traslationResponse.StatusMessage;

                return response; 
            }

            response.Name = pokemonName;
            response.ShakespeareanDescription = traslationResponse.TranslatedText;

            return response;
        }
    }
}
