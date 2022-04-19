//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using PokeApiNet;
    using ShakespeareanPokemonAPI.Services;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShakespeareanPokemonAPI.Models;

    [ApiController]
    [Route("[controller]")]
    public class ShakespeareanPokemonController : ControllerBase
    {
        private readonly ILogger<ShakespeareanPokemonController> Logger;
        private readonly IPokeApiService PokeApiService;

        public ShakespeareanPokemonController
            (
                ILogger<ShakespeareanPokemonController> logger,
                IPokeApiService pokeApiService
            )
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.PokeApiService = pokeApiService ?? throw new ArgumentNullException(nameof(pokeApiService));

        }

        [HttpGet]
        public async Task<IActionResult> Get(string pokemonName)
        {
            try
            {
                PokeResponse rawResponse = await this.PokeApiService.GetPokemonFromApi(pokemonName);

                return new ObjectResult(rawResponse) { StatusCode = rawResponse.ErrorCode};
            }

            catch(Exception ex)
            {
                this.Logger.LogError($"[Operation=Get(ShakespeareanPokemon)], Status=Failed, Message=Exeception thrown: {ex.Message}");

                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }
    }
}
