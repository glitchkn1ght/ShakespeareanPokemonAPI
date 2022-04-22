//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 19/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI.DependancyResolution
{
    using Microsoft.Extensions.DependencyInjection;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Services;
    using ShakespeareanPokemonAPI.Validation;

    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IInputValidator, InputValidator>();
            services.AddScoped<IPokemonDescriptionMapper, PokemonDescriptionMapper>();
            services.AddScoped<ITranslationMapper, ShakespeareTranslationMapper>();
            services.AddScoped<IPokeApiInterpreter, PokeApiInterpreter>();
            services.AddScoped<IFunTranslationsApiInterepreter, FunTranslationsApiInterepreter>();
            services.AddScoped<IPokeApiService, PokeApiService>();
            services.AddScoped<IFunTranslationsApiService, FunTranslationsApiService>();
            services.AddScoped<IShakespeareanPokemonOrchestrator, ShakespeareanPokemonOrchestrator>();
            return services;
        }
    }
}
