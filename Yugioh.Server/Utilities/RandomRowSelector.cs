using Microsoft.EntityFrameworkCore;
using Yugioh.Server.Context;
using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Utilities
{
    public class RandomRowSelector : IRandomRowSelector
    {
        private readonly Random _random;
        private readonly CardsContext _context;

        public RandomRowSelector(CardsContext context)
        {
            _random = new Random();
            _context = context;
        }

        public async Task<object> GetRandomCardAsync()
        {
            var monsterCardsQuery = from monster in _context.MonsterCards
                                    select new CardDto
                                    {
                                        CardId = monster.CardId,
                                        Name = monster.Name,
                                        Type = "Monster"
                                    };

            var spellAndTrapCardsQuery = from spellAndTrap in _context.SpellAndTrapCards
                                         select new CardDto
                                         {
                                             CardId = spellAndTrap.CardId,
                                             Name = spellAndTrap.Name,
                                             Type = "Spell/Trap"
                                         };

            var allCardsQuery = monsterCardsQuery.Concat(spellAndTrapCardsQuery);

            var randomCard = await allCardsQuery.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

            return randomCard;
        }

    }
}
