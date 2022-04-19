//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Services
{
    using PokeApiNet;
    using ShakespeareanPokemonAPI.Models.Config;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using System;
    using ShakespeareanPokemonAPI.BusinessLogic;

    public interface IPokeApiService
    {
        public Task<PokeResponse> GetPokemonFromApi(string pokemonName);
    }

    public class PokeApiService : IPokeApiService
    {
        private readonly HttpClient Client;
        private readonly ConfigSettingsPokeAPI PokeApiConfigSettings;
        private readonly IPokeApiInterpreter PokeApiInterpreter;

        public PokeApiService(HttpClient client, IOptions<ConfigSettingsPokeAPI> pokeApiConfigSettings, IPokeApiInterpreter pokeApiInterpreter)
        {
            this.PokeApiConfigSettings = pokeApiConfigSettings.Value;
            this.Client = client;
            this.Client.BaseAddress = new Uri(PokeApiConfigSettings.BaseUrl);
            this.PokeApiInterpreter = pokeApiInterpreter;
        }

        public async Task<PokeResponse> GetPokemonFromApi(string pokemonName)
        {
            var resource = $"{PokeApiConfigSettings.PokemonResourceUrl}/{pokemonName}";

            PokeResponse pokeResponse = await this.PokeApiInterpreter.InterepretPokeApiResponse(await this.Client.GetAsync(resource), PokeApiConfigSettings.DescriptionLanguage);

            return pokeResponse;
        }
    }
}
