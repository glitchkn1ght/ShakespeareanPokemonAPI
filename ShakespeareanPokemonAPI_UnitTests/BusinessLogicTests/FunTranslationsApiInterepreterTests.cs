//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 20/04/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace ShakespeareanPokemonAPI_UnitTests.BusinesssLogicTests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using ShakespeareanPokemonAPI.BusinessLogic;
    using ShakespeareanPokemonAPI.Mappers;
    using ShakespeareanPokemonAPI.Models.FunTranslationsApi;
    using ShakespeareanPokemonAPI.Models.Responses;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class FunTranslationsApiInterepreterTests
    {
        private Mock<ILogger<FunTranslationsApiInterepreter>> LoggerMock;
        private FunTranslationsApiInterepreter funTranslationsApiInterepreter;
        private Mock<ITranslationMapper> shakespeareTranslationMapperMock;

        [SetUp]
        public void Setup()
        {
            this.LoggerMock = new Mock<ILogger<FunTranslationsApiInterepreter>>();
            this.shakespeareTranslationMapperMock = new Mock<ITranslationMapper>();
            this.funTranslationsApiInterepreter = new FunTranslationsApiInterepreter(this.LoggerMock.Object, this.shakespeareTranslationMapperMock.Object);
        }

        [Test]
        public void WhenConstructorCalledWithNullLogger_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("logger"), delegate
                {
                    this.funTranslationsApiInterepreter = new FunTranslationsApiInterepreter
                    (
                        null,
                        this.shakespeareTranslationMapperMock.Object
                    );
                });
        }

        [Test]
        public void WhenConstructorCalledWithNullMapper_ThenArgNullExceptionThrown()
        {
            Assert.Throws(
                Is.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("translationMapper"), delegate
                {
                    this.funTranslationsApiInterepreter = new FunTranslationsApiInterepreter
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
                    this.funTranslationsApiInterepreter = new FunTranslationsApiInterepreter
                    (
                        this.LoggerMock.Object,
                        this.shakespeareTranslationMapperMock.Object
                    );
                });
        }

        [TestCase("somedescription")]
        public void WhenSuccessCodeReceived_AndMapperReturnsSuccessfully_ThenInterpreterReturnsSuccess(string retString)
        {
            HttpResponseMessage msg = new HttpResponseMessage { Content = null, StatusCode = System.Net.HttpStatusCode.OK };

            this.shakespeareTranslationMapperMock.Setup(x => x.MapTranslation(msg)).Returns(Task.FromResult(retString));

            ResponseStatus expected = new ResponseStatus
            {
                StatusCode = 200,
            };

            TranslationResponse actual = this.funTranslationsApiInterepreter.InterepretFTApiResponse(msg).Result;

            Assert.AreEqual(expected.StatusCode, actual.ResponseStatus.StatusCode);
            Assert.AreEqual("somedescription", actual.TranslatedText);
        }

        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void WhenSuccessCodeReceived_ButMapperReturnsNullOrEmptry_ThenInterpreterReturns404(string retString)
        {            
            HttpResponseMessage msg = new HttpResponseMessage { Content = null, StatusCode = System.Net.HttpStatusCode.OK};

            this.shakespeareTranslationMapperMock.Setup(x => x.MapTranslation(msg)).Returns(Task.FromResult(retString));

            ResponseStatus expected = new ResponseStatus
            {
                StatusCode = 500,
                StatusMessage = "TranslationApi call was sucessful but no description could be mapped"
            };

            TranslationResponse actual = this.funTranslationsApiInterepreter.InterepretFTApiResponse(msg).Result;

            Assert.AreEqual(expected.StatusCode, actual.ResponseStatus.StatusCode);
            Assert.AreEqual(expected.StatusMessage, actual.ResponseStatus.StatusMessage);
        }

        [TestCase(400, "Bad request text is missing")]
        [TestCase(403, "Too many requests, try again in an hour")]
        public void WhenNonSuccessCodeReceived_ThenInterpreterReturnsApiCodeAndMessage(int retCode, string retMsg)
        {
            Error error = new Error()
            {
                ErrorDetail = new ErrorDetail() { Code = retCode, Message = retMsg }
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(error));

            HttpResponseMessage msg = new HttpResponseMessage { Content = content, StatusCode = (System.Net.HttpStatusCode)retCode };

            ResponseStatus expected = new ResponseStatus
            {
                StatusCode = retCode,
                StatusMessage = retMsg
            };

            TranslationResponse actual = this.funTranslationsApiInterepreter.InterepretFTApiResponse(msg).Result;

            Assert.AreEqual(expected.StatusCode, actual.ResponseStatus.StatusCode);
            Assert.AreEqual(expected.StatusMessage, actual.ResponseStatus.StatusMessage);
        }
    }
}