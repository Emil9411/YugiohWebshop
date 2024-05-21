namespace Yugioh.Server.Services.Api
{
    public interface IYugiohApiSingleCard
    {
        Task<string?> YugiohCardByNameAsync(string name);
        Task<string?> YugiohCardByCardIdAsync(int cardId);
    }
}
