//Change History
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
        public Task<string> MapPokemonDescription(HttpResponseMessage httpResponse, string descriptionLanguage);
    }

    public class PokemonDescriptionMapper : IPokemonDescriptionMapper
    {
        public async Task<string> MapPokemonDescription(HttpResponseMessage httpResponse, string descriptionLanguage)
        {
            ServiceResponse mappedResponse = new ServiceResponse();

            var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };

            PokemonSpecies pokemon = JsonConvert.DeserializeObject<PokemonSpecies>(await httpResponse.Content.ReadAsStringAsync(),settings);

            string rawDescription = pokemon?.FlavorTextEntries?.Where(x => x.Language.Name.ToUpper() == descriptionLanguage).FirstOrDefault()?.FlavorText;

            if (!string.IsNullOrEmpty(rawDescription))
            {
                rawDescription = rawDescription.Replace("\n", " ").Replace("\f", " ").Trim();
            }

            return rawDescription;
        }
    }
}
