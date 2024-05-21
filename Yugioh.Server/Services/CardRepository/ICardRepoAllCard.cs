using Yugioh.Server.Model;

namespace Yugioh.Server.Services.CardRepository
{
    public interface ICardRepoAllCard
    {
        Task InitialDatabaseFill(AllCardResponse allCardResponse);
        Task UpdateDatabase(AllCardResponse allCardResponse);
        Task EmptyDatabase();
    }
}
