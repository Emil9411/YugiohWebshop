namespace Yugioh.Server.Services.Api
{
    public class YugiohApi : IYugiohApiAllCard, IYugiohApiSingleCard
    {
        private readonly ILogger<YugiohApi> _logger;

        const string baseUrl = "https://db.ygoprodeck.com/api/v7/cardinfo.php";

        public YugiohApi(ILogger<YugiohApi> logger)
        {
            _logger = logger;
        }

        public async Task<string?> AllYugiohCardAsync()
        {
            using var client = new HttpClient();
            _logger.LogInformation("API: Requesting all yugioh cards");
            var response = await client.GetAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> YugiohCardByNameAsync(string name)
        {
            using var client = new HttpClient();
            _logger.LogInformation($"API: Requesting yugioh card by name: {name}");
            var response = await client.GetAsync($"{baseUrl}?name={name}");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> YugiohCardByCardIdAsync(int cardId)
        {
            using var client = new HttpClient();
            _logger.LogInformation($"API: Requesting yugioh card by id: {cardId}");
            var response = await client.GetAsync($"{baseUrl}?id={cardId}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
