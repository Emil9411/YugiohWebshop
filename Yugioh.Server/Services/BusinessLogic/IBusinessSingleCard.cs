using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Services.BusinessLogic
{
    public interface IBusinessSingleCard
    {
        Task<Card?> GetCardByNameAsync(string name);
        Task<Card?> GetCardByCardIdAsync(int cardId);
    }
}
