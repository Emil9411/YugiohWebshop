using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Yugioh.Server.Controllers;
using Yugioh.Server.Model.CardModels;
using Yugioh.Server.Services.BusinessLogic;
using Yugioh.Server.Utilities;

namespace Yugioh.Server.Test
{
    internal class CardControllerTest
    {
        private readonly Mock<ILogger<CardController>> _loggerMock;
        private readonly Mock<IBusinessAllCard> _businessAllCardMock;
        private readonly Mock<IBusinessSingleCard> _businessSingleCardMock;
        private readonly Mock<IRandomRowSelector> _randomRowSelectorMock;

        public CardControllerTest()
        {
            _loggerMock = new Mock<ILogger<CardController>>();
            _businessAllCardMock = new Mock<IBusinessAllCard>();
            _businessSingleCardMock = new Mock<IBusinessSingleCard>();
            _randomRowSelectorMock = new Mock<IRandomRowSelector>();
        }

        // FillDatabase tests

        [Test]
        public async Task FillDatabase_WhenCalled_ReturnsOk()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.FillDatabase();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task FillDatabase_WhenCalled_LogsInformation()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.FillDatabase();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: All cards processed and saved to the database"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task FillDatabase_WhenCalled_CallsDatabaseFiller()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.FillDatabase();

            // Assert
            _businessAllCardMock.Verify(x => x.DatabaseFiller(), Times.Once);
        }

        // UpdateDatabase tests

        [Test]
        public async Task UpdateDatabase_WhenCalled_ReturnsOk()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.UpdateDatabase();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateDatabase_WhenCalled_LogsInformation()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.UpdateDatabase();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: All cards processed and saved to the database"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task UpdateDatabase_WhenCalled_CallsDatabaseUpdater()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.UpdateDatabase();

            // Assert
            _businessAllCardMock.Verify(x => x.DatabaseUpdater(), Times.Once);
        }

        // CleanDatabase tests

        [Test]
        public async Task CleanDatabase_WhenCalled_ReturnsOk()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.CleanDatabase();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task CleanDatabase_WhenCalled_LogsInformation()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.CleanDatabase();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: Database cleaned"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task CleanDatabase_WhenCalled_CallsDatabaseCleaner()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.CleanDatabase();

            // Assert
            _businessAllCardMock.Verify(x => x.DatabaseCleaner(), Times.Once);
        }

        // GetAllCards tests

        [Test]
        public async Task GetAllCards_WhenCalled_ReturnsOk()
        {
            // Arrange
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllCards();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllCards_WhenCalled_ReturnsAllCards()
        {
            // Arrange
            var allCard = new AllCardResponse();
            _businessAllCardMock.Setup(x => x.GetAllCard()).ReturnsAsync(allCard);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllCards();

            // Assert
            Assert.That((result as OkObjectResult).Value, Is.EqualTo(allCard));
        }

        [Test]
        public async Task GetAllCards_WhenAllCardsIsNull_ReturnsNotFound()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllCard()).ReturnsAsync((AllCardResponse)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllCards();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetAllCards_WhenAllCardsIsNull_LogsError()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllCard()).ReturnsAsync((AllCardResponse)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.GetAllCards();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: Error getting all cards from the database"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task GetAllCards_WhenAllCardsIsNull_ReturnsErrorMessage()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllCard()).ReturnsAsync((AllCardResponse)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllCards();

            // Assert
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Controller: Error getting all cards from the database"));
        }

        // GetAllMonsterCards tests

        [Test]
        public async Task GetAllMonsterCards_WhenCalled_ReturnsOk()
        {
            // Arrange
            var allMonsterCards = new List<MonsterCard>();
            _businessAllCardMock.Setup(x => x.GetAllMonsterCards()).ReturnsAsync(allMonsterCards);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllMonsterCards();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllMonsterCards_WhenCalled_ReturnsAllMonsterCards()
        {
            // Arrange
            var allMonsterCards = new List<MonsterCard>();
            _businessAllCardMock.Setup(x => x.GetAllMonsterCards()).ReturnsAsync(allMonsterCards);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllMonsterCards();

            // Assert
            Assert.That((result as OkObjectResult).Value, Is.EqualTo(allMonsterCards));
        }

        [Test]
        public async Task GetAllMonsterCards_WhenAllMonsterCardsIsNull_ReturnsNotFound()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllMonsterCards()).ReturnsAsync((List<MonsterCard>)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllMonsterCards();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetAllMonsterCards_WhenAllMonsterCardsIsNull_LogsError()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllMonsterCards()).ReturnsAsync((List<MonsterCard>)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.GetAllMonsterCards();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: Error getting all monster cards from the database"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task GetAllMonsterCards_WhenAllMonsterCardsIsNull_ReturnsErrorMessage()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllMonsterCards()).ReturnsAsync((List<MonsterCard>)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllMonsterCards();

            // Assert
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Controller: Error getting all monster cards from the database"));
        }

        // GetAllSpellCards tests

        [Test]
        public async Task GetAllSpellCards_WhenCalled_ReturnsOk()
        {
            // Arrange
            var allSpellCards = new List<SpellAndTrapCard>();
            _businessAllCardMock.Setup(x => x.GetAllSpellCards()).ReturnsAsync(allSpellCards);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllSpellCards();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllSpellCards_WhenCalled_ReturnsAllSpellCards()
        {
            // Arrange
            var allSpellCards = new List<SpellAndTrapCard>();
            _businessAllCardMock.Setup(x => x.GetAllSpellCards()).ReturnsAsync(allSpellCards);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllSpellCards();

            // Assert
            Assert.That((result as OkObjectResult).Value, Is.EqualTo(allSpellCards));
        }

        [Test]
        public async Task GetAllSpellCards_WhenAllSpellCardsIsNull_ReturnsNotFound()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllSpellCards()).ReturnsAsync((List<SpellAndTrapCard>)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllSpellCards();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetAllSpellCards_WhenAllSpellCardsIsNull_LogsError()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllSpellCards()).ReturnsAsync((List<SpellAndTrapCard>)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.GetAllSpellCards();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: Error getting all spell cards from the database"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task GetAllSpellCards_WhenAllSpellCardsIsNull_ReturnsErrorMessage()
        {
            // Arrange
            _businessAllCardMock.Setup(x => x.GetAllSpellCards()).ReturnsAsync((List<SpellAndTrapCard>)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetAllSpellCards();

            // Assert
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Controller: Error getting all spell cards from the database"));
        }

        // GetCardByName tests

        [Test]
        public async Task GetCardByName_WhenCalled_ReturnsOk()
        {
            // Arrange
            var card = new MonsterCard();
            _businessSingleCardMock.Setup(x => x.GetCardByNameAsync(It.IsAny<string>())).ReturnsAsync(card);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardByName(It.IsAny<string>());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetCardByName_WhenCalled_ReturnsCard()
        {
            // Arrange
            var card = new MonsterCard();
            _businessSingleCardMock.Setup(x => x.GetCardByNameAsync(It.IsAny<string>())).ReturnsAsync(card);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardByName(It.IsAny<string>());

            // Assert
            Assert.That((result as OkObjectResult).Value, Is.EqualTo(card));
        }

        [Test]
        public async Task GetCardByName_WhenCardIsNull_ReturnsNotFound()
        {
            // Arrange
            _businessSingleCardMock.Setup(x => x.GetCardByNameAsync(It.IsAny<string>())).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardByName(It.IsAny<string>());

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetCardByName_WhenCardIsNull_LogsError()
        {
            // Arrange
            _businessSingleCardMock.Setup(x => x.GetCardByNameAsync(It.IsAny<string>())).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.GetCardByName(It.IsAny<string>());

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: Card not found in the database or the API"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task GetCardByName_WhenCardIsNull_ReturnsErrorMessage()
        {
            // Arrange
            _businessSingleCardMock.Setup(x => x.GetCardByNameAsync(It.IsAny<string>())).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardByName(It.IsAny<string>());

            // Assert
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Controller: Card not found in the database or the API"));
        }

        // GetCardById tests

        [Test]
        public async Task GetCardById_WhenCalled_ReturnsOk()
        {
            // Arrange
            var card = new MonsterCard();
            _businessSingleCardMock.Setup(x => x.GetCardByCardIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardById(It.IsAny<int>());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetCardById_WhenCalled_ReturnsCard()
        {
            // Arrange
            var card = new MonsterCard();
            _businessSingleCardMock.Setup(x => x.GetCardByCardIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardById(It.IsAny<int>());

            // Assert
            Assert.That((result as OkObjectResult).Value, Is.EqualTo(card));
        }

        [Test]
        public async Task GetCardById_WhenCardIsNull_ReturnsNotFound()
        {
            // Arrange
            _businessSingleCardMock.Setup(x => x.GetCardByCardIdAsync(It.IsAny<int>())).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardById(It.IsAny<int>());

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetCardById_WhenCardIsNull_LogsError()
        {
            // Arrange
            _businessSingleCardMock.Setup(x => x.GetCardByCardIdAsync(It.IsAny<int>())).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.GetCardById(It.IsAny<int>());

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller: Card not found in the database or the API"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task GetCardById_WhenCardIsNull_ReturnsErrorMessage()
        {
            // Arrange
            _businessSingleCardMock.Setup(x => x.GetCardByCardIdAsync(It.IsAny<int>())).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetCardById(It.IsAny<int>());

            // Assert
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Controller: Card not found in the database or the API"));
        }

        // GetRandomCard tests

        [Test]
        public async Task GetRandomCard_WhenCalled_ReturnsOk()
        {
            // Arrange
            var card = new MonsterCard();
            _randomRowSelectorMock.Setup(x => x.GetRandomCardAsync()).ReturnsAsync(card);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetRandomCard();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetRandomCard_WhenCalled_ReturnsCard()
        {
            // Arrange
            var card = new MonsterCard();
            _randomRowSelectorMock.Setup(x => x.GetRandomCardAsync()).ReturnsAsync(card);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetRandomCard();

            // Assert
            Assert.That((result as OkObjectResult).Value, Is.EqualTo(card));
        }

        [Test]
        public async Task GetRandomCard_WhenCardIsNull_ReturnsNotFound()
        {
            // Arrange
            _randomRowSelectorMock.Setup(x => x.GetRandomCardAsync()).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetRandomCard();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetRandomCard_WhenCardIsNull_LogsError()
        {
            // Arrange
            _randomRowSelectorMock.Setup(x => x.GetRandomCardAsync()).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            await controller.GetRandomCard();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Controller(Random): Random Card not found"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task GetRandomCard_WhenCardIsNull_ReturnsErrorMessage()
        {
            // Arrange
            _randomRowSelectorMock.Setup(x => x.GetRandomCardAsync()).ReturnsAsync((MonsterCard)null);
            var controller = new CardController(_loggerMock.Object, _businessAllCardMock.Object, _businessSingleCardMock.Object, _randomRowSelectorMock.Object);

            // Act
            var result = await controller.GetRandomCard();

            // Assert
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Controller(Random): Random Card not found"));
        }
    }
}
