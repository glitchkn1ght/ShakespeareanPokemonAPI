//Change History
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
                PokeResponse PokemonResponse = await this.ShakespeareanPokemonOrchestrator.GetShakespeareanPokemon(pokemonName);

                return new ObjectResult(PokemonResponse) { StatusCode = PokemonResponse.StatusCode};
            }

            catch(Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(ShakespeareanPokemon)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }
    }
}
