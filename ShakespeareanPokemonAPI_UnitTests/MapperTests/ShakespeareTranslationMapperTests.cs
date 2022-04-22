//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.MapperTests
{
    using Newtonsoft.Json;
    using NUnit.Framework;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.FunTranslationsApi;
    using System.Net.Http;

    public class ShakespeareTranslationMapperTests
    {
        private ShakespeareTranslationMapper shakespeareTranslationMapper;
        private Translation translation;

        [SetUp]
        public void Setup()
        {
            this.shakespeareTranslationMapper = new ShakespeareTranslationMapper();

            this.translation = new Translation()
            {
                TranslationSuccess = new TranslationSuccess() { Total = 1 },
                TranslationContents = new TranslationContents()
                {
                    OriginalText = "What is a man but a miserable pile of secrets?",
                    TranslatedText = "What is a sir but a miserable pile of secrets?",
                    TranlastionType = "shakespeare"
                }
            };

        }

        [TestCase("What is a sir but a miserable pile of secrets?")]
        public void WhenApiResponseContainsValidReponse_ThenMapperReturnsDescriptionAsExpected(string expectedTranslation)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(this.translation));
            HttpResponseMessage msg = new HttpResponseMessage { Content = content };

            string actualTranslation = this.shakespeareTranslationMapper.MapTranslation(msg).Result;

            Assert.AreEqual(expectedTranslation, actualTranslation);
        }

        [Test]
        public void WhenTranslationull_ThenMapperReturnsNull()
        {
            HttpResponseMessage msg = new HttpResponseMessage { Content = null };

            string actualTranslation = this.shakespeareTranslationMapper.MapTranslation(msg).Result;

            Assert.AreEqual(null, actualTranslation);
        }
    }
}