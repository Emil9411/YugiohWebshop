namespace Yugioh.Server.Services.Api
{
    public interface IYugiohApiAllCard
    {
        Task<string?> AllYugiohCardAsync();
    }
}
