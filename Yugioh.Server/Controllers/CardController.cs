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

        [HttpGet("filldatabase")]
        public async Task<ActionResult> FillDatabase()
        {
            await _businessAllCard.DatabaseFiller();
            _logger.LogInformation("Controller: All cards processed and saved to the database");
            return Ok("Controller(Fill): All cards processed and saved to the database");
        }

        [HttpGet("updatedatabase")]
        public async Task<ActionResult> UpdateDatabase()
        {
            await _businessAllCard.DatabaseUpdater();
            _logger.LogInformation("Controller: All cards processed and saved to the database");
            return Ok("Controller(Update): All cards processed and saved to the database");
        }

        [HttpGet("cleandatabase")]
        public async Task<ActionResult> CleanDatabase()
        {
            await _businessAllCard.DatabaseCleaner();
            _logger.LogInformation("Controller: Database cleaned");
            return Ok("Controller: Database cleaned");
        }

        [HttpGet("cardbyname")]
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

        [HttpGet("cardbyid")]
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
