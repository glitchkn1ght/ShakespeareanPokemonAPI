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

    public interface IFunTranslationsApiService
    {
        public Task<TraslationResponse> TranslatePokemonDescription(string descriptionInModernEnglish);
    }

    public class FunTranslationsApiService : IFunTranslationsApiService
    {
        private readonly ILogger<FunTranslationsApiService> Logger;
        private readonly HttpClient Client;
        private readonly ConfigSettingsFunTranslationsAPI FTApiConfigSettings;
        private readonly IFunTranslationsApiInterepreter FTApiInterpreter;

        public FunTranslationsApiService(ILogger<FunTranslationsApiService> logger, HttpClient client, IOptions<ConfigSettingsFunTranslationsAPI> fTApiConfigSettings, IFunTranslationsApiInterepreter ftApiInterpreter)
        {
            this.Logger = logger;
            this.FTApiConfigSettings = fTApiConfigSettings.Value;
            this.Client = client;
            this.Client.BaseAddress = new Uri(FTApiConfigSettings.BaseUrl);
            this.FTApiInterpreter = ftApiInterpreter;
        }

        public async Task<TraslationResponse> TranslatePokemonDescription(string descriptionInModernEnglish)
        {
            var resource = $"{FTApiConfigSettings.ShakespeareTranslateResourceUrl}";

            this.Logger.LogInformation($"[Operation=TranslatePokemonDescription], Status=Success, Message=Calling to PokeApi at {this.Client.BaseAddress+resource}");

            TraslationResponse translationResponse = await this.FTApiInterpreter.InterepretFTApiResponse(await this.Client.GetAsync(resource));

            return translationResponse;
        }
    }
}
