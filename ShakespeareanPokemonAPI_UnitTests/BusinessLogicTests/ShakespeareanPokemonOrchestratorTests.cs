//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.BusinesssLogicTests
{
    using Moq;
    using NUnit.Framework;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Models.Responses;
    using ShakespeareanPokemonAPI.Services;
    using System;
    using System.Threading.Tasks;

    public class ShakespeareanPokemonOrchestratorTests
    {
        private Mock<IFunTranslationsApiService> FTServiceMock;
        private Mock<IPokeApiService> pokeServiceMock;
        private ShakespeareanPokemonOrchestrator shakespeareanPokemonOrchestrator;

        [SetUp]
        public void Setup()
        {
            this.FTServiceMock = new Mock<IFunTranslationsApiService>();
            this.pokeServiceMock = new Mock<IPokeApiService>();

            this.shakespeareanPokemonOrchestrator = new ShakespeareanPokemonOrchestrator(this.FTServiceMock.Object, this.pokeServiceMock.Object);
        }

        [Test]
        public void WhenConstructorCalledWithNullFTService_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("ftApiService"), delegate
                {
                    this.shakespeareanPokemonOrchestrator = new ShakespeareanPokemonOrchestrator
                    (
                        null,
                        this.pokeServiceMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullPokeApiService_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("pokeApiService"), delegate
                {
                    this.shakespeareanPokemonOrchestrator = new ShakespeareanPokemonOrchestrator
                    (
                        this.FTServiceMock.Object,
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
                    this.shakespeareanPokemonOrchestrator = new ShakespeareanPokemonOrchestrator
                    (
                        this.FTServiceMock.Object,
                        this.pokeServiceMock.Object
                    );
                });
        }

        [Test]
        public void WhenBothServicesSuccess_ThenReturnPokemonAndSuccessCode()
        {
            ServiceResponse pokeServiceResponse = new ServiceResponse()
            {
                ReturnedText = "Spits fire that is hot enough to melt boulders. Known to cause forest fires unintentionally.",
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = true
                }
            };

            ServiceResponse translationServiceResponse = new ServiceResponse()
            {
                ReturnedText = "Spits fire yond is hot enow to melt boulders. Known to cause forest fires unintentionally.",
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = true,
                    StatusCode = 200
                }
            };

            this.pokeServiceMock.Setup(x => x.GetPokemonFromApi("Charizard")).Returns(Task.FromResult(pokeServiceResponse));

            this.FTServiceMock.Setup(x => x.TranslatePokemonDescription("Spits fire that is hot enough to melt boulders. Known to cause forest fires unintentionally.")).Returns(Task.FromResult(translationServiceResponse));

            ShakespeareanPokemonResponse orchestratorResponse = this.shakespeareanPokemonOrchestrator.GetShakespeareanPokemon("Charizard").Result;

            Assert.AreEqual(true, orchestratorResponse.ResponseStatus.IsSuccess);
            Assert.AreEqual(200, orchestratorResponse.ResponseStatus.StatusCode);
            Assert.AreEqual("Spits fire yond is hot enow to melt boulders. Known to cause forest fires unintentionally.", orchestratorResponse.Pokemon.Description);
        }

        [TestCase(404,"Not Found")]
        [TestCase(400, "Bad Request")]
        public void WhenPokeAPIServiceNonSuccess_ThenReturnFailureCodeAndMessage(int retCode, string retMessage)
        {
            ServiceResponse pokeServiceResponse = new ServiceResponse()
            {
                ResponseStatus = new ResponseStatus()
                {
                    StatusCode = retCode,
                    StatusMessage = retMessage
                }
            };

            this.pokeServiceMock.Setup(x => x.GetPokemonFromApi("Ditto")).Returns(Task.FromResult(pokeServiceResponse));

            ShakespeareanPokemonResponse actualResponse = this.shakespeareanPokemonOrchestrator.GetShakespeareanPokemon("Ditto").Result;

            Assert.AreEqual(retCode, actualResponse.ResponseStatus.StatusCode);
            Assert.AreEqual(retMessage, actualResponse.ResponseStatus.StatusMessage);
        }

        [TestCase(404, "Not Found")]
        [TestCase(400, "Bad Request")]
        [TestCase(500, "TranslationApi call was sucessful but no description could be mapped")]
        public void WhenPokeAPIServiceSuccess_ButFTServiceReturnsNonSuccess_ThenReturnFailureCodeAndMessage(int retCode, string retMessage)
        {
            ServiceResponse pokeServiceResponse = new ServiceResponse()
            {
                ReturnedText = "Ditto has always freaked me out a bit",
                
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = true,
                    StatusCode = 200
                }
            };

            ServiceResponse translationServiceResponse = new ServiceResponse()
            {
                ResponseStatus = new ResponseStatus()
                {
                    StatusCode = retCode,
                    StatusMessage = retMessage
                }
            };

            this.pokeServiceMock.Setup(x => x.GetPokemonFromApi("Ditto")).Returns(Task.FromResult(pokeServiceResponse));
            this.FTServiceMock.Setup(x => x.TranslatePokemonDescription("Ditto has always freaked me out a bit")).Returns(Task.FromResult(translationServiceResponse));

            ShakespeareanPokemonResponse orchestratorResponse = this.shakespeareanPokemonOrchestrator.GetShakespeareanPokemon("Ditto").Result;

            Assert.AreEqual(retCode, orchestratorResponse.ResponseStatus.StatusCode);
            Assert.AreEqual(retMessage, orchestratorResponse.ResponseStatus.StatusMessage);
        }
    }
}