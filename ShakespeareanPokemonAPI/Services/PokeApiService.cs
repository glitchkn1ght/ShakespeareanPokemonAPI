//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Services
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PokeApiNet;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Models.Config;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IPokeApiService
    {
        public Task<ServiceResponse> GetPokemonFromApi(string pokemonName);
    }

    public class PokeApiService : IPokeApiService
    {
        private readonly ILogger<PokeApiService> Logger;
        private readonly HttpClient Client;
        private readonly ConfigSettingsPokeAPI PokeApiConfigSettings;
        private readonly IPokeApiInterpreter PokeApiInterpreter;

        public PokeApiService(ILogger<PokeApiService> logger, HttpClient client, IOptions<ConfigSettingsPokeAPI> pokeApiConfigSettings, IPokeApiInterpreter pokeApiInterpreter)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.PokeApiConfigSettings = pokeApiConfigSettings.Value;
            this.Client = client ?? throw new ArgumentNullException(nameof(client));
            this.Client.BaseAddress = new Uri(PokeApiConfigSettings.BaseUrl);
            this.PokeApiInterpreter = pokeApiInterpreter ?? throw new ArgumentNullException(nameof(pokeApiInterpreter));
        }

        public async Task<ServiceResponse> GetPokemonFromApi(string pokemonName)
        {
            var resource = $"{PokeApiConfigSettings.PokemonResourceUrl}/{pokemonName}";

            this.Logger.LogInformation($"[Operation=GetPokemonFromApi], Status=Success, Message=Calling to PokeApi at {this.Client.BaseAddress+resource}");

            ServiceResponse pokeResponse = await this.PokeApiInterpreter.InterepretPokeApiResponse(await this.Client.GetAsync(resource), PokeApiConfigSettings.DescriptionLanguage);

            return pokeResponse;
        }
    }
}
