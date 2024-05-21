using Yugioh.Server.Model;

namespace Yugioh.Server.Services.JsonProcess
{
    public interface IJsonProcessSingleCard
    {
        Card? GetCard(string json);
    }
}
