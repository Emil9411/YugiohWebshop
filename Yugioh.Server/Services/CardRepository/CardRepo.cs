using Microsoft.EntityFrameworkCore;
using Yugioh.Server.Context;
using Yugioh.Server.Model;

namespace Yugioh.Server.Services.CardRepository
{
    public class CardRepo : ICardRepoAllCard, ICardRepoSingleCard
    {
        private readonly CardsContext _context;
        private readonly ILogger<CardRepo> _logger;

        public CardRepo(CardsContext context, ILogger<CardRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitialDatabaseFill(AllCardResponse allCardResponse)
        {
            if (allCardResponse.Error)
            {
                _logger.LogError("Repository: Error in the response from the API");
                return;
            }
            if (!_context.MonsterCards.Any() && !_context.SpellAndTrapCards.Any())
            {
                foreach (var monsterCard in allCardResponse.MonsterCards)
                {
                    _context.MonsterCards.Add(monsterCard);
                }

                foreach (var spellAndTrapCard in allCardResponse.SpellAndTrapCards)
                {
                    _context.SpellAndTrapCards.Add(spellAndTrapCard);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Repository: Initial database fill complete");
            }
        }

        public async Task UpdateDatabase(AllCardResponse allCardResponse)
        {
            if (allCardResponse.Error)
            {
                _logger.LogError("Repository: Error in the response from the API");
                return;
            }
            var monsterAdded = 0;
            var spellAndTrapAdded = 0;
            var monsterUpdated = 0;
            var spellAndTrapUpdated = 0;
            foreach (var monsterCard in allCardResponse.MonsterCards)
            {
                // If the card's id is not in the database, add it
                if (!_context.MonsterCards.Any(c => c.CardId == monsterCard.CardId))
                {
                    _context.MonsterCards.Add(monsterCard);
                    monsterAdded++;
                }
                // If the card in the database is different from the card in the response, update it
                else
                {
                    var cardInDatabase = await _context.MonsterCards.FirstOrDefaultAsync(c => c.CardId == monsterCard.CardId);
                    if (cardInDatabase != null && !cardInDatabase.Equals(monsterCard))
                    {
                        _context.MonsterCards.Remove(cardInDatabase);
                        _context.MonsterCards.Add(monsterCard);
                        monsterUpdated++;
                    }
                }
            }
            foreach (var spellAndTrapCard in allCardResponse.SpellAndTrapCards)
            {
                // If the card's id is not in the database, add it
                if (!_context.SpellAndTrapCards.Any(c => c.CardId == spellAndTrapCard.CardId))
                {
                    _context.SpellAndTrapCards.Add(spellAndTrapCard);
                    spellAndTrapAdded++;
                }
                // If the card in the database is different from the card in the response, update it
                else
                {
                    var cardInDatabase = await _context.SpellAndTrapCards.FirstOrDefaultAsync(c => c.CardId == spellAndTrapCard.CardId);
                    if (cardInDatabase != null && !cardInDatabase.Equals(spellAndTrapCard))
                    {
                        _context.SpellAndTrapCards.Remove(cardInDatabase);
                        _context.SpellAndTrapCards.Add(spellAndTrapCard);
                        spellAndTrapUpdated++;
                    }
                }
            }
            _logger.LogInformation($"Repository: Added {monsterAdded} new monster cards to the database");
            _logger.LogInformation($"Repository: Added {spellAndTrapAdded} new spell and trap cards to the database");
            _logger.LogInformation($"Repository: Updated {monsterUpdated} monster cards in the database");
            _logger.LogInformation($"Repository: Updated {spellAndTrapUpdated} spell and trap cards in the database");
            await _context.SaveChangesAsync();
            _logger.LogInformation("Repository: Database update complete");
        }

        public async Task EmptyDatabase()
        {
            _context.MonsterCards.RemoveRange(_context.MonsterCards);
            _context.SpellAndTrapCards.RemoveRange(_context.SpellAndTrapCards);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Repository: Database emptied");
        }

        public async Task<AllCardResponse> GetAllCard()
        {
            var allCardResponse = new AllCardResponse
            {
                MonsterCards = await _context.MonsterCards.ToListAsync(),
                SpellAndTrapCards = await _context.SpellAndTrapCards.ToListAsync()
            };
            _logger.LogInformation("Repository: All cards retrieved from the database");
            return allCardResponse;
        }

        public async Task<List<MonsterCard>> GetAllMonsterCards()
        {
            var monsterCards = await _context.MonsterCards.ToListAsync();
            _logger.LogInformation("Repository: All monster cards retrieved from the database");
            return monsterCards;
        }

        public async Task<List<SpellAndTrapCard>> GetAllSpellCards()
        {
            var spellAndTrapCards = await _context.SpellAndTrapCards.ToListAsync();
            _logger.LogInformation("Repository: All spell and trap cards retrieved from the database");
            return spellAndTrapCards;
        }

        public async Task<Card?> GetCardByNameAsync(string name)
        {
            var monsterCard = await _context.MonsterCards.FirstOrDefaultAsync(c => c.Name == name);
            if (monsterCard != null)
            {
                return monsterCard;
            }
            var spellAndTrapCard = await _context.SpellAndTrapCards.FirstOrDefaultAsync(c => c.Name == name);
            if (spellAndTrapCard != null)
            {
                return spellAndTrapCard;
            }
            return null;
        }

        public async Task<Card?> GetCardByCardIdAsync(int cardId)
        {
            var monsterCard = await _context.MonsterCards.FirstOrDefaultAsync(c => c.CardId == cardId);
            if (monsterCard != null)
            {
                return monsterCard;
            }
            var spellAndTrapCard = await _context.SpellAndTrapCards.FirstOrDefaultAsync(c => c.CardId == cardId);
            if (spellAndTrapCard != null)
            {
                return spellAndTrapCard;
            }
            return null;
        }

        public async Task<Card?> AddCardAsync(Card card)
        {
            if (card is MonsterCard monsterCard)
            {
                _context.MonsterCards.Add(monsterCard);
                await _context.SaveChangesAsync();
                return monsterCard;
            }
            if (card is SpellAndTrapCard spellAndTrapCard)
            {
                _context.SpellAndTrapCards.Add(spellAndTrapCard);
                await _context.SaveChangesAsync();
                return spellAndTrapCard;
            }
            return null;
        }
    }
}
