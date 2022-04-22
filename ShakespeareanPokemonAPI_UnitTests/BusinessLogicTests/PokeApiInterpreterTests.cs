//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.BusinesssLogicTests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PokeApiInterpreterTests
    {
        private Mock<ILogger<PokeApiInterpreter>> LoggerMock;
        private PokeApiInterpreter pokeApiInterpreter;
        private Mock<IPokemonDescriptionMapper> pokemonDescriptionMapperMock;


        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<PokeApiInterpreter>>();
            this.pokemonDescriptionMapperMock = new Mock<IPokemonDescriptionMapper>();
            this.pokeApiInterpreter = new PokeApiInterpreter(this.LoggerMock.Object, this.pokemonDescriptionMapperMock.Object);
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.pokeApiInterpreter = new PokeApiInterpreter
                    (
                        null,
                        this.pokemonDescriptionMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("descriptionMapper"), delegate
                {
                    this.pokeApiInterpreter = new PokeApiInterpreter
                    (
                        this.LoggerMock.Object,
                        null
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithValidArguements_ThenNoExceptionThrown()
        {
            Assert.DoesNotThrow(
                delegate
                {
                    this.pokeApiInterpreter = new PokeApiInterpreter
                    (
                        this.LoggerMock.Object,
                        this.pokemonDescriptionMapperMock.Object
                    );
                });
        }

        [TestCase("somedescription")]
        public void WhenSuccessCodeReceived_AndMapperReturnsSuccessfully_ThenInterpreterReturnsSuccess(string retString)
        {
            HttpResponseMessage msg = new HttpResponseMessage { Content = null, StatusCode = System.Net.HttpStatusCode.OK };

            this.pokemonDescriptionMapperMock.Setup(x => x.MapPokemonDescription(msg, It.IsAny<string>())).Returns(Task.FromResult(retString));

            ResponseStatus expected = new ResponseStatus
            {
                StatusCode = 200,
            };

            ServiceResponse actual = this.pokeApiInterpreter.InterepretPokeApiResponse(msg, It.IsAny<string>()).Result;

            Assert.AreEqual(expected.StatusCode, actual.ResponseStatus.StatusCode);
            Assert.AreEqual("somedescription", actual.ReturnedText);
        }

        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void WhenSuccessCodeReceived_ButMapperReturnsNullOrEmptry_ThenInterpreterReturns404(string retString)
        {            
            HttpResponseMessage msg = new HttpResponseMessage { Content = null, StatusCode = System.Net.HttpStatusCode.OK};

            this.pokemonDescriptionMapperMock.Setup(x => x.MapPokemonDescription(msg, It.IsAny<string>())).Returns(Task.FromResult(retString));

            ResponseStatus expected = new ResponseStatus
            {
                StatusCode = 500,
                StatusMessage = "Pokemon was found on PokeApi but no valid description could be mapped."
            };

            ServiceResponse actual = this.pokeApiInterpreter.InterepretPokeApiResponse(msg, It.IsAny<string>()).Result;

            Assert.AreEqual(expected.StatusCode, actual.ResponseStatus.StatusCode);
            Assert.AreEqual(expected.StatusMessage, actual.ResponseStatus.StatusMessage);
        }

        [TestCase(400)]
        [TestCase(403)]
        public void WhenNonSuccessCodeReceived_ThenInterpreterReturnsApiCode(int retCode)
        {
            HttpResponseMessage msg = new HttpResponseMessage { Content = null, StatusCode = (System.Net.HttpStatusCode)retCode };

            ResponseStatus expected = new ResponseStatus
            {
                StatusCode = retCode,
                StatusMessage = "Could not retrieve Pokemon from PokeApi endpoint."
            };

            ServiceResponse actual = this.pokeApiInterpreter.InterepretPokeApiResponse(msg, It.IsAny<string>()).Result;

            Assert.AreEqual(expected.StatusCode, actual.ResponseStatus.StatusCode);
            Assert.AreEqual(expected.StatusMessage, actual.ResponseStatus.StatusMessage);
        }
    }
}