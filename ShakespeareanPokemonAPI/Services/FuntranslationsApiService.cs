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
    using System.Text.Encodings;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.Models.FunTranslationsApi;
    using Newtonsoft.Json;
    using System.Text;

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

            var json = JsonConvert.SerializeObject(new PostBody(descriptionInModernEnglish));
            var data = new StringContent(json, Encoding.UTF8, this.FTApiConfigSettings.ContentType);

            this.Logger.LogInformation($"[Operation=TranslatePokemonDescription], Status=Success, Message=Calling to FunTranslationsApi at {this.Client.BaseAddress+resource}");

            TraslationResponse translationResponse = await this.FTApiInterpreter.InterepretFTApiResponse(await this.Client.PostAsync(resource,data));

            return translationResponse;
        }
    }
}
