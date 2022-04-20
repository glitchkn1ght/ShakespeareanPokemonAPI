﻿//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.BusinessLogic
{
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IPokeApiInterpreter
    {
        public Task<PokeApiResponse> InterepretPokeApiResponse(HttpResponseMessage pokeApiResponse, string descriptionLanguage);
    }

    public class PokeApiInterpreter : IPokeApiInterpreter
    {
        private readonly ILogger<PokeApiInterpreter> Logger;
        IPokemonDescriptionMapper DescriptionMapper;

        public PokeApiInterpreter(ILogger<PokeApiInterpreter> logger, IPokemonDescriptionMapper descriptionMapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.DescriptionMapper = descriptionMapper ?? throw new ArgumentNullException(nameof(descriptionMapper));
        }

        public async Task<PokeApiResponse> InterepretPokeApiResponse(HttpResponseMessage ApiResponse, string descriptionLanguage)
        {
            PokeApiResponse pokeApiResponse = new PokeApiResponse();

            if (ApiResponse.IsSuccessStatusCode)
            {
                this.Logger.LogInformation($"[Operation=InterepretPokeApiResponse], Status=Success, Message=Success code received from PokeApi endpoint, mapping description.");

                pokeApiResponse = await this.DescriptionMapper.MapPokemonDescription(ApiResponse, descriptionLanguage);

                if (!string.IsNullOrWhiteSpace(pokeApiResponse.PokemonDescription))
                {
                    this.Logger.LogInformation($"[Operation=InterepretPokeApiResponse], Status=Success, Message=Successfully mapped description from response");

                    pokeApiResponse.ResponseStatus.IsSuccess = true;
                }

                else
                {
                    this.Logger.LogWarning($"[Operation=InterepretPokeApiResponse], Status=Success, Message=Could not map description from response");

                    pokeApiResponse.ResponseStatus.StatusCode = 404;
                    pokeApiResponse.ResponseStatus.StatusMessage = "Pokemon was found on Api but no valid description could be mapped";
                }
            }

            else
            {
                this.Logger.LogWarning($"[Operation=InterepretPokeApiResponse], Status=Failure, Message=Failure code received from PokeApi endpoint, mapping error");

                pokeApiResponse.ResponseStatus.StatusCode = (int)ApiResponse.StatusCode;
                pokeApiResponse.ResponseStatus.StatusMessage = "Could not retrieve Pokemon from PokeApi";
            }

            return pokeApiResponse;
        }
    }
}
