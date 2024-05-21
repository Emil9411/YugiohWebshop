using Yugioh.Server.Model;

namespace Yugioh.Server.Services.JsonProcess
{
    public interface IJsonProcessAllCard
    {
        AllCardResponse AllCardProcess(string json);
    }
}
