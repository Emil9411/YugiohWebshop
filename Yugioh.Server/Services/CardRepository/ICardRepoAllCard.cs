﻿using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Services.CardRepository
{
    public interface ICardRepoAllCard
    {
        Task InitialDatabaseFill(AllCardResponse allCardResponse);
        Task UpdateDatabase(AllCardResponse allCardResponse);
        Task EmptyDatabase();
        Task<AllCardResponse> GetAllCard();
        Task<List<MonsterCard>> GetAllMonsterCards();
        Task<List<SpellAndTrapCard>> GetAllSpellCards();
    }
}
