//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Services
{
    using PokeApiNet;
    using ShakespeareanPokemonAPI.Models.Config;
    using ShakespeareanPokemonAPI.Models;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using System;

    public interface IPokeApiService
    {
        public Task<PokemonSpecies> GetPokemonDescription(string pokemonName);
    }

    public class PokeApiService : IPokeApiService
    {
        private readonly PokeApiClient Client;
        private readonly ConfigSettingsPokeAPI configSettings;

        public PokeApiService(HttpClient httpClient, IOptions<ConfigSettingsPokeAPI> configPokeApiSettings)
        {
            this.configSettings = configPokeApiSettings.Value;
            httpClient.BaseAddress = new Uri(configSettings.BaseUrl);
            this.Client = new PokeApiClient(httpClient);
        }


        public async Task<PokemonSpecies> GetPokemonDescription(string pokemonName)
        {
            var resource = $"{configSettings.PokemonResourceUrl}/{pokemonName}";

            PokemonSpecies species = await this.Client.GetResourceAsync<PokemonSpecies>(pokemonName);

            return species;
        }
    }
}
