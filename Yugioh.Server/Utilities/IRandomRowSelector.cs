namespace Yugioh.Server.Utilities
{
    public interface IRandomRowSelector
    {
        Task<object> GetRandomCardAsync();
    }
}
