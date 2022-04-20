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
    using Microsoft.Extensions.Logging;

    public interface IPokeApiService
    {
        public Task<PokeApiResponse> GetPokemonFromApi(string pokemonName);
    }

    public class PokeApiService : IPokeApiService
    {
        private readonly ILogger<PokeApiService> Logger;
        private readonly HttpClient Client;
        private readonly ConfigSettingsPokeAPI PokeApiConfigSettings;
        private readonly IPokeApiInterpreter PokeApiInterpreter;

        public PokeApiService(ILogger<PokeApiService> logger, HttpClient client, IOptions<ConfigSettingsPokeAPI> pokeApiConfigSettings, IPokeApiInterpreter pokeApiInterpreter)
        {
            this.Logger = logger;
            this.PokeApiConfigSettings = pokeApiConfigSettings.Value;
            this.Client = client;
            this.Client.BaseAddress = new Uri(PokeApiConfigSettings.BaseUrl);
            this.PokeApiInterpreter = pokeApiInterpreter;
        }

        public async Task<PokeApiResponse> GetPokemonFromApi(string pokemonName)
        {
            var resource = $"{PokeApiConfigSettings.PokemonResourceUrl}/{pokemonName}";

            this.Logger.LogInformation($"[Operation=GetPokemonFromApi], Status=Success, Message=Calling to PokeApi at {this.Client.BaseAddress+resource}");

            PokeApiResponse pokeResponse = await this.PokeApiInterpreter.InterepretPokeApiResponse(await this.Client.GetAsync(resource), PokeApiConfigSettings.DescriptionLanguage);

            return pokeResponse;
        }
    }
}
