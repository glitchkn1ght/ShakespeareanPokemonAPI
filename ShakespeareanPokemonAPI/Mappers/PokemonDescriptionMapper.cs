﻿//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Mappers
{
    using Newtonsoft.Json;
    using PokeApiNet;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IPokemonDescriptionMapper
    {
        public Task<PokeResponse> MapPokemonDescription(HttpResponseMessage httpResponse, string descriptionLanguage);
    }

    public class PokemonDescriptionMapper : IPokemonDescriptionMapper
    {
        public async Task<PokeResponse> MapPokemonDescription(HttpResponseMessage httpResponse, string descriptionLanguage)
        {
            PokeResponse mappedResponse = new PokeResponse();

            PokemonSpecies pokemon = JsonConvert.DeserializeObject<PokemonSpecies>(await httpResponse.Content.ReadAsStringAsync());

            mappedResponse.PokemonName = pokemon.Name;

            string rawDescription = pokemon.FlavorTextEntries.Where(x => x.Language.Name.ToUpper() == descriptionLanguage).FirstOrDefault()?.FlavorText;

            mappedResponse.PokemonDescription = rawDescription.Replace("\n", " ").Replace("\f"," ");

            return mappedResponse;
        }
    }
}
