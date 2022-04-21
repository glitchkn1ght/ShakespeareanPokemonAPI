//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.FunTranslationsApi;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IFunTranslationsApiInterepreter
    {
        public Task<TranslationResponse> InterepretFTApiResponse(HttpResponseMessage ApiResponse);
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

        public async Task<TranslationResponse> InterepretFTApiResponse(HttpResponseMessage ApiResponse)
        {
            TranslationResponse translationResponse = new TranslationResponse();

            if (ApiResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=InterepretFTApiResponse], Status=Success, Message=Success code received from FunTranslations endpoint, mapping translation.");

                translationResponse.TranslatedText = await this.TranslationMapper.MapTranslation(ApiResponse);

                if (!string.IsNullOrWhiteSpace(translationResponse.TranslatedText))
                {
                    this.Logger.LogInformation($"[Operation=InterepretFTApiResponse], Status=Success, Message=Successfully mapped translation from response");

                    translationResponse.ResponseStatus.StatusCode = 200;
                    translationResponse.ResponseStatus.IsSuccess = true;
                }

                else
                {
                    this.Logger.LogWarning($"[Operation=InterepretFTApiResponse], Status=Failure, Message=Could not map translation from response");

                    translationResponse.ResponseStatus.StatusCode = 500;
                    translationResponse.ResponseStatus.StatusMessage = "TranslationApi call was sucessful but no description could be mapped";
                }
            }

            else
            {
                this.Logger.LogWarning($"[Operation=InterepretFTApiResponse], Status=Failure, Message=Failure code received from TranslationAPI endpoint");

                Error error = JsonConvert.DeserializeObject<Error>(await ApiResponse.Content.ReadAsStringAsync());

                translationResponse.ResponseStatus.StatusCode = error.ErrorDetail.Code;
                translationResponse.ResponseStatus.StatusMessage = error.ErrorDetail.Message;
            }

            return translationResponse;
        }
    }
}
