//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.FunTranslationsApi;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IFunTranslationsApiInterepreter
    {
        public Task<TraslationResponse> InterepretFTApiResponse(HttpResponseMessage ApiResponse);
    }

    public class FunTranslationsApiInterepreter : IFunTranslationsApiInterepreter
    {
        private readonly ILogger<FunTranslationsApiInterepreter> Logger;
        private readonly ITranslationMapper TranslationMapper;

        public FunTranslationsApiInterepreter(ILogger<FunTranslationsApiInterepreter> logger, ITranslationMapper translationMapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.TranslationMapper = translationMapper ?? throw new ArgumentNullException(nameof(translationMapper));
        }

        public async Task<TraslationResponse> InterepretFTApiResponse(HttpResponseMessage ApiResponse)
        {
            TraslationResponse translationResponse = new TraslationResponse();

            if (ApiResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=InterepretFTApiResponse], Status=Success, Message=Success code received from FunTranslations endpoint, mapping translation.");

                Translation translation = await this.TranslationMapper.MapTranslation(ApiResponse);

                if (!string.IsNullOrWhiteSpace(translation?.TranslationContents?.TranslatedText))
                {
                    this.Logger.LogInformation($"[Operation=InterepretFTApiResponse], Status=Success, Message=Successfully mapped translation from response");

                    translationResponse.TranslatedText = translation.TranslationContents.TranslatedText;
                    translationResponse.IsSuccess = true;
                }

                else
                {
                    this.Logger.LogWarning($"[Operation=InterepretFTApiResponse], Status=Success, Message=Could not map translation from response");

                    translationResponse.StatusCode = 500;
                    translationResponse.StatusMessage = "TranslationApi call was sucessful but no valid description could be mapped";
                }
            }

            else
            {
                this.Logger.LogWarning($"[Operation=InterepretFTApiResponse], Status=Failure, Message=Failure code received from PokeApi endpoint, mapping error");

                //todo
            }

            return translationResponse;
        }
    }
}
