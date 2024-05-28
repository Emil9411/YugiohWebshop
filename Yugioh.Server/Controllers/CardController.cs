using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yugioh.Server.Services.BusinessLogic;
using Yugioh.Server.Utilities;

namespace Yugioh.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IBusinessAllCard _businessAllCard;
        private readonly IBusinessSingleCard _businessSingleCard;
        private readonly IRandomRowSelector _randomRowSelector;

        public CardController(ILogger<CardController> logger, IBusinessAllCard business, IBusinessSingleCard businessSingleCard, IRandomRowSelector randomRowSelector)
        {
            _logger = logger;
            _businessAllCard = business;
            _businessSingleCard = businessSingleCard;
            _randomRowSelector = randomRowSelector;
        }

        [HttpGet("filldatabase"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> FillDatabase()
        {
            await _businessAllCard.DatabaseFiller();
            _logger.LogInformation("Controller: All cards processed and saved to the database");
            return Ok("Controller(Fill): All cards processed and saved to the database");
        }

        [HttpPatch("updatedatabase"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDatabase()
        {
            await _businessAllCard.DatabaseUpdater();
            _logger.LogInformation("Controller: All cards processed and saved to the database");
            return Ok("Controller(Update): All cards processed and saved to the database");
        }

        [HttpDelete("cleandatabase"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> CleanDatabase()
        {
            await _businessAllCard.DatabaseCleaner();
            _logger.LogInformation("Controller: Database cleaned");
            return Ok("Controller: Database cleaned");
        }

        [HttpGet("allcards")]
        public async Task<ActionResult> GetAllCards()
        {
            var allCard = await _businessAllCard.GetAllCard();
            if (allCard == null)
            {
                _logger.LogError("Controller: Error getting all cards from the database");
                return NotFound("Controller: Error getting all cards from the database");
            }
            _logger.LogInformation("Controller: All cards found");
            return Ok(allCard);
        }

        [HttpGet("allmonstercards"), Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> GetAllMonsterCards()
        {
            var allMonsterCards = await _businessAllCard.GetAllMonsterCards();
            if (allMonsterCards == null)
            {
                _logger.LogError("Controller: Error getting all monster cards from the database");
                return NotFound("Controller: Error getting all monster cards from the database");
            }
            _logger.LogInformation("Controller: All monster cards found");
            return Ok(allMonsterCards);
        }

        [HttpGet("allspellcards"), Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> GetAllSpellCards()
        {
            var allSpellCards = await _businessAllCard.GetAllSpellCards();
            if (allSpellCards == null)
            {
                _logger.LogError("Controller: Error getting all spell cards from the database");
                return NotFound("Controller: Error getting all spell cards from the database");
            }
            _logger.LogInformation("Controller: All spell cards found");
            return Ok(allSpellCards);
        }

        [HttpGet("cardbyname"), Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> GetCardByName([FromQuery] string name)
        {
            var card = await _businessSingleCard.GetCardByNameAsync(name);
            if (card == null)
            {
                _logger.LogError("Controller: Card not found in the database or the API");
                return NotFound("Controller: Card not found in the database or the API");
            }
            _logger.LogInformation("Controller: Card found");
            return Ok(card);
        }

        [HttpGet("cardbyid"), Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> GetCardById([FromQuery] int cardId)
        {
            var card = await _businessSingleCard.GetCardByCardIdAsync(cardId);
            if (card == null)
            {
                _logger.LogError("Controller: Card not found in the database or the API");
                return NotFound("Controller: Card not found in the database or the API");
            }
            _logger.LogInformation("Controller: Card found");
            return Ok(card);
        }

        [HttpGet("randomcard")]
        public async Task<ActionResult> GetRandomCard()
        {
            var randomCard = await _randomRowSelector.GetRandomCardAsync();
            if (randomCard == null)
            {
                _logger.LogError("Controller(Random): Random Card not found");
                return NotFound("Controller(Random): Random Card not found");
            }
            _logger.LogInformation("Controller(Random): Random Card found");
            return Ok(randomCard);
        }
    }
}
