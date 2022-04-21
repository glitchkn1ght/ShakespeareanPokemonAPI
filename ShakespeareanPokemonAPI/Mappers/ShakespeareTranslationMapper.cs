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
        public Task<string> MapTranslation(HttpResponseMessage httpResponse);
    }

    public class ShakespeareTranslationMapper : ITranslationMapper
    {
        public async Task<string> MapTranslation(HttpResponseMessage httpResponse)
        {
            Translation translation = new Translation();

            var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };

            translation = JsonConvert.DeserializeObject<Translation>(await httpResponse.Content.ReadAsStringAsync(), settings);

            string translatedText = translation?.TranslationContents?.TranslatedText;

            if (!string.IsNullOrEmpty(translatedText))
            {
                translatedText = translatedText.Replace("\n", " ").Replace("\f", " ").Trim();
            }

            return translatedText;
        }
    }
}
