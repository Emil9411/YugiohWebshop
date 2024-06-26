﻿using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Services.CardRepository
{
    public interface ICardRepoSingleCard
    {
        Task<Card?> GetCardByNameAsync(string name);
        Task<Card?> GetCardByCardIdAsync(int cardId);
        Task<Card?> AddCardAsync(Card card);
    }
}
