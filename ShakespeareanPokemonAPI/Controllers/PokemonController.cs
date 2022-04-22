//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Models.Responses;
    using ShakespeareanPokemonAPI.Validation;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> Logger;
        private readonly IInputValidator InputValidator;
        private readonly IShakespeareanPokemonOrchestrator ShakespeareanPokemonOrchestrator;
      
        public PokemonController
            (
                ILogger<PokemonController> logger,
                IInputValidator inputValidator,
                IShakespeareanPokemonOrchestrator shakespeareanPokemonOrchestrator
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
            this.ShakespeareanPokemonOrchestrator = shakespeareanPokemonOrchestrator ?? throw new ArgumentNullException(nameof(shakespeareanPokemonOrchestrator));
        }

        /// <summary> Gets the pokemons name and it's flavour text translated into shakespearean</summary>
        /// <param name="pokemonName"> The name of the pokemon you wish to retrieve data for</param>
        /// <response code="200">Returns the pokemons data.</response>
        /// <response code="400">If the request parameters are malformed, or if the inputted PokemonName is invalid</response>
        /// <response code="404">If the request URL on eiter of the Apis cannot be found on the server.</response>  
        /// <response code="403">If you have exceeded the usage limit on TranslationApi</response>  
        /// <response code="500">Internal application Error.</response>  
        [HttpGet("{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShakespeareanPokemon))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseStatus))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResponseStatus))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseStatus))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseStatus))]
        public async Task<IActionResult> Get(string pokemonName)
        {
            try
            {
                if (!this.InputValidator.ValidateInput(pokemonName))
                {
                    return new ObjectResult(new ResponseStatus(false, 400, $"PokemonName {pokemonName} could not be validated. Please check your input.")) { StatusCode = 400 };
                }
                
                this.Logger.LogInformation($"[Operation=ShakespeareanPokemonController(Get)], Status=Success, Message=Input Validated. Attempting to retrieve and map description for pokemon {pokemonName}");

                ShakespeareanPokemonResponse response = await this.ShakespeareanPokemonOrchestrator.GetShakespeareanPokemon(pokemonName);

                if (response.ResponseStatus.IsSuccess)
                {
                    this.Logger.LogInformation($"[Operation=ShakespeareanPokemonController(Get)], Status=Success, Message=Attempting to retrieve and map description for pokemon {pokemonName}");

                    return new OkObjectResult(response.Pokemon);
                }

                this.Logger.LogWarning($"[Operation=ShakespeareanPokemonController(Get)], Status=Failure, Message=Unable to retrieve and map description for pokemon {pokemonName}, details: {response.ResponseStatus}");

                return new ObjectResult(response.ResponseStatus) {StatusCode = response.ResponseStatus.StatusCode };
            }

            catch(Exception ex)
            {
                this.Logger.LogError($"[Operation=ShakespeareanPokemonController(Get)], Status=Failed, Message=Exeception thrown, Details: {ex.Message}");

                return new ObjectResult(new ResponseStatus(false, 500, "Internal Application Error Has Occurred")) { StatusCode = 500 };
            }
        }
    }
}
