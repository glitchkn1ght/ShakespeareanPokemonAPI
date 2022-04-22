//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.BusinesssLogicTests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Controllers;
    using ShakespeareanPokemonAPI.Models.Responses;
    using ShakespeareanPokemonAPI.Validation;
    using System;
    using System.Threading.Tasks;

    public class ShakespeareanPokemonControllerTests
    {
        private Mock<ILogger<PokemonController>> LoggerMock;
        private Mock<IInputValidator> inputValidatorMock;
        private Mock<IShakespeareanPokemonOrchestrator> shakespeareanPokemonOrchestratorMock;
        private PokemonController shakespeareanPokemonController;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<PokemonController>>();
            this.inputValidatorMock = new Mock<IInputValidator>();
            this.shakespeareanPokemonOrchestratorMock = new Mock<IShakespeareanPokemonOrchestrator>();
            this.shakespeareanPokemonController = new PokemonController(this.LoggerMock.Object, this.inputValidatorMock.Object, this.shakespeareanPokemonOrchestratorMock.Object);
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.shakespeareanPokemonController = new PokemonController
                    (
                        null,
                        this.inputValidatorMock.Object,
                        this.shakespeareanPokemonOrchestratorMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullValidator_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("inputValidator"), delegate
                {
                    this.shakespeareanPokemonController = new PokemonController
                    (
                        this.LoggerMock.Object,
                        null,
                        this.shakespeareanPokemonOrchestratorMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullOrchestrator_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("shakespeareanPokemonOrchestrator"), delegate
                {
                    this.shakespeareanPokemonController = new PokemonController
                    (
                        this.LoggerMock.Object,
                        this.inputValidatorMock.Object,
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
                    this.shakespeareanPokemonController = new PokemonController
                    (
                        this.LoggerMock.Object,
                        this.inputValidatorMock.Object,
                        this.shakespeareanPokemonOrchestratorMock.Object
                    );
                });
        }

        [Test]
        public void WhenOrchestratorSuccess_ThenReturnPokemonAndSuccessCode()
        {
            ShakespeareanPokemonResponse expectedResponse = new ShakespeareanPokemonResponse()
            {
                Pokemon = new ShakespeareanPokemon()
                {
                    Name = "Psyduck",
                    Description = "Forsooth, it's like looking in a mirror."
                },
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                }
            };

            this.inputValidatorMock.Setup(x => x.ValidateInput("Psyduck")).Returns(true);
            this.shakespeareanPokemonOrchestratorMock.Setup(x => x.GetShakespeareanPokemon("Psyduck")).Returns(Task.FromResult(expectedResponse));

             ObjectResult actual = (ObjectResult)this.shakespeareanPokemonController.Get("Psyduck").Result;

            Assert.AreEqual(expectedResponse.ResponseStatus.StatusCode, actual.StatusCode);
            Assert.AreEqual(expectedResponse.Pokemon.Name, ((ShakespeareanPokemon)actual.Value).Name);
            Assert.AreEqual(expectedResponse.Pokemon.Description, ((ShakespeareanPokemon)actual.Value).Description);
        }

        [Test]
        public void WhenValidatorNonSuccess_ThenReturnFailureCodeAndMessage()
        {
            ShakespeareanPokemonResponse expectedResponse = new ShakespeareanPokemonResponse()
            {
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = $"PokemonName {"\"\""} could not be validated. Please check your input."
                }
            };

            this.inputValidatorMock.Setup(x => x.ValidateInput("\"\"")).Returns(false);

            ObjectResult actual = (ObjectResult)this.shakespeareanPokemonController.Get("\"\"").Result;

            Assert.AreEqual(expectedResponse.ResponseStatus.StatusCode, actual.StatusCode);
            Assert.AreEqual(expectedResponse.ResponseStatus.StatusMessage, ((ResponseStatus)actual.Value).StatusMessage);
        }

        [TestCase(404,"Not Found")]
        [TestCase(400, "Bad Request")]
        [TestCase(403, "Too many requests, try again in an hour.")]
        [TestCase(403, "Some internal error.")]
        public void WhenOrchestratorNonSuccess_ThenReturnFailureCodeAndMessage(int retCode, string retMessage)
        {
            ShakespeareanPokemonResponse expectedResponse = new ShakespeareanPokemonResponse()
            {
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = false,
                    StatusCode = retCode,
                    StatusMessage = retMessage
                }
            };

            this.inputValidatorMock.Setup(x => x.ValidateInput("Psyduck")).Returns(true);
            this.shakespeareanPokemonOrchestratorMock.Setup(x => x.GetShakespeareanPokemon("Psyduck")).Returns(Task.FromResult(expectedResponse));

            ObjectResult actual = (ObjectResult)this.shakespeareanPokemonController.Get("Psyduck").Result;

            Assert.AreEqual(expectedResponse.ResponseStatus.StatusCode, actual.StatusCode);
            Assert.AreEqual(expectedResponse.ResponseStatus.StatusMessage, ((ResponseStatus)actual.Value).StatusMessage);
        }

        [Test]
        public void WhenExceptionThrown_ThenReturnInternalServerError()
        {
            this.inputValidatorMock.Setup(x => x.ValidateInput("Psyduck")).Returns(true);

            this.shakespeareanPokemonOrchestratorMock.Setup(x => x.GetShakespeareanPokemon("Psyduck")).Throws(new Exception());

            ObjectResult actual = (ObjectResult)this.shakespeareanPokemonController.Get("Psyduck").Result;

            Assert.AreEqual(500, actual.StatusCode);
            Assert.AreEqual("Internal Application Error Has Occurred", ((ResponseStatus)actual.Value).StatusMessage);
        }
    }
}