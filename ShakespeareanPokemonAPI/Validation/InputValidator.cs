//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.Validation
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text.RegularExpressions;

    public interface IInputValidator
    {
        public bool ValidateInput(string PokemonName);
    }

    public class InputValidator : IInputValidator
    {
        private readonly ILogger<InputValidator> Logger;

        public InputValidator(ILogger<InputValidator> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool ValidateInput(string pokemonName)
        {
            //Looks for strings with no alphanumeric characters. Could be better.
            Regex regEx = new Regex("^[^a-zA-Z0-9]*$");

            MatchCollection match = regEx.Matches(pokemonName);

            if (string.IsNullOrEmpty(pokemonName) || match.Count > 0)
            {
                this.Logger.LogWarning($"[Operation=ValidateInput], Status=Failure, Message=Could not validate inputted PokemonName: {pokemonName}");
                return false;
            }

            return true;
        }
    }
}
