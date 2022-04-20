﻿//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class ShakespeareanPokemonController : ControllerBase
    {
        private readonly ILogger<ShakespeareanPokemonController> Logger;
        private readonly IShakespeareanPokemonOrchestrator ShakespeareanPokemonOrchestrator;

        public ShakespeareanPokemonController
            (
                ILogger<ShakespeareanPokemonController> logger,
                IShakespeareanPokemonOrchestrator shakespeareanPokemonOrchestrator
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.ShakespeareanPokemonOrchestrator = shakespeareanPokemonOrchestrator ?? throw new ArgumentNullException(nameof(shakespeareanPokemonOrchestrator));
        }

        [HttpGet]
        public async Task<IActionResult> Get(string pokemonName)
        {
            try
            {
                this.Logger.LogInformation($"[Operation=ShakespeareanPokemonController(Get)], Status=Success, Message=Attempting to retrieve and map description for pokemon {pokemonName}");

                ShakespeareanPokemonResponse response = await this.ShakespeareanPokemonOrchestrator.GetShakespeareanPokemon(pokemonName);

                if (response.ResponseStatus.IsSuccess)
                {
                    this.Logger.LogInformation($"[Operation=ShakespeareanPokemonController(Get)], Status=Success, Message=Attempting to retrieve and map description for pokemon {pokemonName}");

                    return new OkObjectResult(response.Pokemon);
                }

                this.Logger.LogWarning($"[Operation=ShakespeareanPokemonController(Get)], Status=Failure, Message=Unable to retrieve and map description for pokemon {pokemonName}, details: {response.ResponseStatus}");

                return new ObjectResult(response.ResponseStatus);
            }

            catch(Exception ex)
            {
                this.Logger.LogError($"[Operation=ShakespeareanPokemonController(Get)], Status=Failed, Message=Exeception thrown");

                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }
    }
}
