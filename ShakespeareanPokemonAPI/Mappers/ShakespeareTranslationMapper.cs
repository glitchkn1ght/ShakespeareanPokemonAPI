//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Mappers
{
    using Newtonsoft.Json;
    using ShakespeareanPokemonAPI.Models.FunTranslationsApi;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ITranslationMapper
    {
        public Task<Translation> MapTranslation(HttpResponseMessage httpResponse);
    }

    public class ShakespeareTranslationMapper : ITranslationMapper
    {
        public async Task<Translation> MapTranslation(HttpResponseMessage httpResponse)
        {
            Translation translation = new Translation();

            translation = JsonConvert.DeserializeObject<Translation>(await httpResponse.Content.ReadAsStringAsync());

            translation.TranslationContents.TranslatedText = translation.TranslationContents.TranslatedText.Replace("\n", " ").Replace("\f"," ");

            return translation;
        }
    }
}
