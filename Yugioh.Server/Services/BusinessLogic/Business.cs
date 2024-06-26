﻿using Yugioh.Server.Model.CardModels;
using Yugioh.Server.Services.Api;
using Yugioh.Server.Services.CardRepository;
using Yugioh.Server.Services.JsonProcess;

namespace Yugioh.Server.Services.BusinessLogic
{
    public class Business : IBusinessAllCard, IBusinessSingleCard
    {
        private readonly ILogger<Business> _logger;
        private readonly IYugiohApiAllCard _yugiohApiAllCard;
        private readonly IYugiohApiSingleCard _yugiohApiSingleCard;
        private readonly IJsonProcessAllCard _allCardJsonProcess;
        private readonly IJsonProcessSingleCard _singleCardJsonProcess;
        private readonly ICardRepoAllCard _cardRepoAllCard;
        private readonly ICardRepoSingleCard _cardRepoSingleCard;

        public Business(IYugiohApiAllCard yugiohApi, IJsonProcessAllCard allCardJsonProcess, ILogger<Business> logger, ICardRepoAllCard cardRepo, IYugiohApiSingleCard yugiohApiSingleCard, IJsonProcessSingleCard singleCardJsonProcess, ICardRepoSingleCard cardRepoSingleCard)
        {
            _yugiohApiAllCard = yugiohApi;
            _allCardJsonProcess = allCardJsonProcess;
            _logger = logger;
            _cardRepoAllCard = cardRepo;
            _yugiohApiSingleCard = yugiohApiSingleCard;
            _singleCardJsonProcess = singleCardJsonProcess;
            _cardRepoSingleCard = cardRepoSingleCard;
        }

        public async Task<AllCardResponse> DatabaseFiller()
        {
            var json = await _yugiohApiAllCard.AllYugiohCardAsync();
            var allCardResponse = _allCardJsonProcess.AllCardProcess(json);
            await _cardRepoAllCard.InitialDatabaseFill(allCardResponse);
            _logger.LogInformation("Business: All cards processed and saved to the database");
            return allCardResponse;
        }

        public async Task<AllCardResponse> DatabaseUpdater()
        {
            var json = await _yugiohApiAllCard.AllYugiohCardAsync();
            var allCardResponse = _allCardJsonProcess.AllCardProcess(json);
            await _cardRepoAllCard.UpdateDatabase(allCardResponse);
            _logger.LogInformation("Business: All cards processed and saved to the database");
            return allCardResponse;
        }

        public async Task DatabaseCleaner()
        {
            await _cardRepoAllCard.EmptyDatabase();
            _logger.LogInformation("Business: Database cleaned");
        }

        public async Task<AllCardResponse> GetAllCard()
        {
            var allCardResponse = await _cardRepoAllCard.GetAllCard();
            _logger.LogInformation("Business: All cards retrieved from the database");
            return allCardResponse;
        }

        public async Task<List<MonsterCard>> GetAllMonsterCards()
        {
            var monsterCards = await _cardRepoAllCard.GetAllMonsterCards();
            _logger.LogInformation("Business: All monster cards retrieved from the database");
            return monsterCards;
        }

        public async Task<List<SpellAndTrapCard>> GetAllSpellCards()
        {
            var spellAndTrapCards = await _cardRepoAllCard.GetAllSpellCards();
            _logger.LogInformation("Business: All spell and trap cards retrieved from the database");
            return spellAndTrapCards;
        }

        public async Task<Card?> GetCardByNameAsync(string name)
        {
            var card = await _cardRepoSingleCard.GetCardByNameAsync(name);
            if (card == null)
            {
                _logger.LogInformation("Business: Card not found in the database, requesting from the API");
                var json = await _yugiohApiSingleCard.YugiohCardByNameAsync(name);
                card = _singleCardJsonProcess.GetCard(json);
                await _cardRepoSingleCard.AddCardAsync(card);
                _logger.LogInformation("Business: Card processed and saved to the database");
            }
            _logger.LogInformation($"Business: Card found in the database: {card.Name}");
            return card;
        }

        public async Task<Card?> GetCardByCardIdAsync(int cardId)
        {
            var card = await _cardRepoSingleCard.GetCardByCardIdAsync(cardId);
            if (card == null)
            {
                _logger.LogInformation("Business: Card not found in the database, requesting from the API");
                var json = await _yugiohApiSingleCard.YugiohCardByCardIdAsync(cardId);
                card = _singleCardJsonProcess.GetCard(json);
                await _cardRepoSingleCard.AddCardAsync(card);
                _logger.LogInformation("Business: Card processed and saved to the database");
            }
            _logger.LogInformation($"Business: Card found in the database: {card.Name}");
            return card;
        }
    }
}