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
    using System;
    using System.Threading.Tasks;

    public class ShakespeareanPokemonControllerTests
    {
        private Mock<ILogger<ShakespeareanPokemonController>> LoggerMock;
        private Mock<IShakespeareanPokemonOrchestrator> shakespeareanPokemonOrchestratorMock;
        private ShakespeareanPokemonController shakespeareanPokemonController;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<ShakespeareanPokemonController>>();
            this.shakespeareanPokemonOrchestratorMock = new Mock<IShakespeareanPokemonOrchestrator>();
            this.shakespeareanPokemonController = new ShakespeareanPokemonController(this.LoggerMock.Object, this.shakespeareanPokemonOrchestratorMock.Object);
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.shakespeareanPokemonController = new ShakespeareanPokemonController
                    (
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
                    this.shakespeareanPokemonController = new ShakespeareanPokemonController
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
                    this.shakespeareanPokemonController = new ShakespeareanPokemonController
                    (
                        this.LoggerMock.Object,
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
                    ShakespeareanDescription = "Forsooth, it's like looking in a mirror."
                },
                ResponseStatus = new ResponseStatus()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                }
            };

            this.shakespeareanPokemonOrchestratorMock.Setup(x => x.GetShakespeareanPokemon("Psyduck")).Returns(Task.FromResult(expectedResponse));

             ObjectResult actual = (ObjectResult)this.shakespeareanPokemonController.Get("Psyduck").Result;

            Assert.AreEqual(expectedResponse.ResponseStatus.StatusCode, actual.StatusCode);
            Assert.AreEqual(expectedResponse.Pokemon.Name, ((ShakespeareanPokemon)actual.Value).Name);
            Assert.AreEqual(expectedResponse.Pokemon.ShakespeareanDescription, ((ShakespeareanPokemon)actual.Value).ShakespeareanDescription);
        }

        [Test]
        public void WhenExceptionThrown_ThenReturnInternalServerError()
        {
            this.shakespeareanPokemonOrchestratorMock.Setup(x => x.GetShakespeareanPokemon("Psyduck")).Throws(new Exception());

            ObjectResult actual = (ObjectResult)this.shakespeareanPokemonController.Get("Psyduck").Result;

            Assert.AreEqual(500, actual.StatusCode);
            Assert.AreEqual("Internal Application Error Has Occurred", actual.Value);
        }
    }
}