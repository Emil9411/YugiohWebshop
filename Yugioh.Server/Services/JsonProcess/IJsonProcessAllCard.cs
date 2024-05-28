using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Services.JsonProcess
{
    public interface IJsonProcessAllCard
    {
        AllCardResponse AllCardProcess(string json);
    }
}
