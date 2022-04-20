//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.MapperTests
{
    using Newtonsoft.Json;
    using NUnit.Framework;
    using PokeApiNet;
    using ShakespeareanPokemonAPI.Mappers;
    using System.Collections.Generic;
    using System.Net.Http;

    public class PokemonDescriptionMapperTests
    {
        private PokemonDescriptionMapper pokemonDescriptionMapper;
        private PokemonSpecies testPokemon;
        private List<PokemonSpeciesFlavorTexts> flavourTexts;

        [SetUp]
        public void Setup()
        {
            this.pokemonDescriptionMapper = new PokemonDescriptionMapper();

            this.flavourTexts = new List<PokemonSpeciesFlavorTexts>()
            {
                new PokemonSpeciesFlavorTexts()
                {
                  Version = new NamedApiResource<Version>(){Name = "Red"},
                  Language = new NamedApiResource<Language>(){Name = "EN"},
                  FlavorText = "Some random flavor text 1"
                },

                new PokemonSpeciesFlavorTexts()
                {
                  Version = new NamedApiResource<Version>(){Name = "Red"},
                  Language = new NamedApiResource<Language>(){Name = "fr" },
                  FlavorText = "Un texte de saveur aléatoire 2"
                },

                new PokemonSpeciesFlavorTexts()
                {
                  Version = new NamedApiResource<Version>(){Name = "Red"},
                  Language = new NamedApiResource<Language>(){Name = "JP"},
                  FlavorText = "いくつかのランダムなフレーバーテキスト 3"
                },
            };


            this.testPokemon = new PokemonSpecies()
            {
                Names = new List<Names>
                {
                    new Names() { Name = "Pikachu" }
                },
                FlavorTextEntries = this.flavourTexts
            };
        }

        [TestCase("EN", "Some random flavor text 1")]
        [TestCase("FR", "Un texte de saveur aléatoire 2")]
        [TestCase("JP", "いくつかのランダムなフレーバーテキスト 3")]
        public void WhenApiResponseContainsValidPokemon_ThenMapperReturnsDescriptionAsExpected(string lang, string expectedFlavour)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.testPokemon));
            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            string actualFlavour = this.pokemonDescriptionMapper.MapPokemonDescription(msg, lang).Result;

            Assert.AreEqual(expectedFlavour, actualFlavour);
        }

        [Test]
        public void WhenPokemonNull_ThenMapperReturnsNull()
        {
           // HttpContent content = new StringContent(JsonConvert.SerializeObject(this.testPokemon));
            HttpResponseMessage msg = new HttpResponseMessage { Content = null };

            string actualFlavour = this.pokemonDescriptionMapper.MapPokemonDescription(msg, "EN").Result;

            Assert.AreEqual(null, actualFlavour);
        }

        [Test]
        public void WhenPokemonFlavorTextListNull_ThenMapperReturnsNull()
        {
            this.testPokemon.FlavorTextEntries = null;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.testPokemon));
            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            string actualFlavour = this.pokemonDescriptionMapper.MapPokemonDescription(msg, "EN").Result;

            Assert.AreEqual(null, actualFlavour);
        }

        [Test]
        public void WhenPokemonFlavorTextListEmpty_ThenMapperReturnsNull()
        {
            this.testPokemon.FlavorTextEntries = new List<PokemonSpeciesFlavorTexts>() ;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.testPokemon));
            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            string actualFlavour = this.pokemonDescriptionMapper.MapPokemonDescription(msg, "EN").Result;

            Assert.AreEqual(null, actualFlavour);
        }

        [Test]
        public void WhenPokemonFlavorTextListDoesNotContainLanguageSpecified_ThenMapperReturnsNull()
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.testPokemon));
            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            string actualFlavour = this.pokemonDescriptionMapper.MapPokemonDescription(msg, "elvish").Result;

            Assert.AreEqual(null, actualFlavour);
        }

        [Test]
        public void WhenPokemonFlavorTextContainsExtraneousCharacters_ThenMapperRemovesTheseCharacters()
        {
            this.testPokemon.FlavorTextEntries[0].FlavorText = "\n flavour text....\nwith extra stuff!\f";
            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.testPokemon));
            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            string actualFlavour = this.pokemonDescriptionMapper.MapPokemonDescription(msg, "EN").Result;

            Assert.AreEqual("flavour text.... with extra stuff!", actualFlavour);
        }
    }
}