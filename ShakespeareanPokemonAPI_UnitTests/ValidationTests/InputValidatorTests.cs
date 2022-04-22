//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.ValidationTests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using ShakespeareanPokemonAPI.Validation;
    using System;

    public class InputValidatorTests
    {
        private Mock<ILogger<InputValidator>> LoggerMock;
        private InputValidator inputValidator;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<InputValidator>>();
            this.inputValidator = new InputValidator(this.LoggerMock.Object);
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.inputValidator = new InputValidator
                    (
                        null
                    );
                });
        }

        [TestCase("")]
        [TestCase("\"\"")]
        [TestCase("'#")]
        public void WhenStringContainsNoAlphaNumChars_ThenReturnFalse(string stringToValidate)
        {
            bool result = this.inputValidator.ValidateInput(stringToValidate);

            Assert.IsFalse(result);
        }

        [TestCase("ditto")]
        [TestCase("charizard")]
        [TestCase("gary5")]
        public void WhenStringValid_ThenReturnFalse(string stringToValidate)
        {
            bool result = this.inputValidator.ValidateInput(stringToValidate);

            Assert.IsTrue(result);
        }
    }
}
