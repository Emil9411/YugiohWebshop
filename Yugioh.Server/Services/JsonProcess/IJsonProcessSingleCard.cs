using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Services.JsonProcess
{
    public interface IJsonProcessSingleCard
    {
        Card? GetCard(string json);
    }
}
