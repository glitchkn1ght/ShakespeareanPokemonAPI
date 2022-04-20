//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
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
        private readonly IFunTranslationsApiService FTApiService;
        private readonly IPokeApiService PokeApiService;

        public ShakespeareanPokemonOrchestrator
        (
            IFunTranslationsApiService ftApiService,
            IPokeApiService pokeApiService
        )
        {
            this.FTApiService = ftApiService ?? throw new ArgumentNullException(nameof(ftApiService));
            this.PokeApiService = pokeApiService ?? throw new ArgumentNullException(nameof(pokeApiService));
        }

        public async Task<ShakespeareanPokemonResponse> GetShakespeareanPokemon(string pokemonName)
        {
            ShakespeareanPokemonResponse response = new ShakespeareanPokemonResponse();

            PokeApiResponse pokeApiResponse = await this.PokeApiService.GetPokemonFromApi(pokemonName);

            if (!pokeApiResponse.ResponseStatus.IsSuccess)
            {
                response.ResponseStatus.StatusCode = pokeApiResponse.ResponseStatus.StatusCode;
                response.ResponseStatus.StatusMessage = pokeApiResponse.ResponseStatus.StatusMessage;
                
                return response;
            }

            TraslationResponse traslationResponse = await this.FTApiService.TranslatePokemonDescription(pokeApiResponse.PokemonDescription);

            if(!traslationResponse.ResponseStatus.IsSuccess)
            {
                response.ResponseStatus.StatusCode = traslationResponse.ResponseStatus.StatusCode;
                response.ResponseStatus.StatusMessage = traslationResponse.ResponseStatus.StatusMessage;

                return response; 
            }

            response.ResponseStatus.IsSuccess = true;
            response.Pokemon.Name = pokemonName;
            response.Pokemon.ShakespeareanDescription = traslationResponse.TranslatedText;

            return response;
        }
    }
}
