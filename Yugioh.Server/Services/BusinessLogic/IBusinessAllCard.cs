using Yugioh.Server.Model;

namespace Yugioh.Server.Services.BusinessLogic
{
    public interface IBusinessAllCard
    {
        Task<AllCardResponse> DatabaseFiller();
        Task<AllCardResponse> DatabaseUpdater();
        Task DatabaseCleaner();
        Task<AllCardResponse> GetAllCard();
        Task<List<MonsterCard>> GetAllMonsterCards();
        Task<List<SpellAndTrapCard>> GetAllSpellCards();
    }
}
