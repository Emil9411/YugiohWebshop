using System.Text.Json;
using Yugioh.Server.Model;

namespace Yugioh.Server.Services.JsonProcess
{
    public class YugiohJsonProcessor : IJsonProcessAllCard, IJsonProcessSingleCard
    {
        private readonly ILogger<YugiohJsonProcessor> _logger;

        public YugiohJsonProcessor(ILogger<YugiohJsonProcessor> logger)
        {
            _logger = logger;
        }

        readonly List<string> priceStrings = new() { "tcgplayer_price", "ebay_price", "amazon_price", "coolstuffinc_price" };

        public AllCardResponse AllCardProcess(string json)
        {
            var allCardResponse = new AllCardResponse();
            if (string.IsNullOrEmpty(json))
            {
                _logger.LogError("JsonProcessor: Empty response from the API");
                return allCardResponse;
            }

            var document = JsonDocument.Parse(json);

            // Check if error exists
            if (document.RootElement.TryGetProperty("error", out _))
            {
                allCardResponse.Error = true;
                _logger.LogError("JsonProcessor: Error in the response from the API");
                return allCardResponse;
            }

            if (!document.RootElement.TryGetProperty("data", out var root))
            {
                _logger.LogError("JsonProcessor: No 'data' property in the response from the API");
                return allCardResponse;
            }

            foreach (var card in root.EnumerateArray())
            {
                try
                {
                    // Process each card with the ProcessSingleCard method
                    var processedCard = ProcessSingleCard(card);
                    if (processedCard != null)
                    {
                        // Decide if the card is a monster, spell, or trap card
                        if (processedCard is MonsterCard monsterCard)
                        {
                            allCardResponse.MonsterCards.Add(monsterCard);
                        }
                        else if (processedCard is SpellAndTrapCard spellAndTrapCard)
                        {
                            allCardResponse.SpellAndTrapCards.Add(spellAndTrapCard);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"JsonProcessor: Error processing card: {e.Message}");
                }
            }

            _logger.LogInformation("JsonProcessor: All cards processed");
            return allCardResponse;
        }

        public Card? GetCard(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                _logger.LogError("JsonProcessor: Empty response from the API");
                return null;
            }

            var document = JsonDocument.Parse(json);

            // Check if error exists
            if (document.RootElement.TryGetProperty("error", out _))
            {
                _logger.LogError("JsonProcessor: Error in the response from the API");
                return null;
            }

            if (!document.RootElement.TryGetProperty("data", out var root))
            {
                // If there is no 'data' property, it means that the json is for a single card
                _logger.LogInformation("JsonProcessor: Single card processed");
                return ProcessSingleCard(root);
            }

            // If there is a 'data' property, it means that the json is an array of a single card
            _logger.LogInformation("JsonProcessor: Single card processed");
            return ProcessSingleCard(root[0]);
        }

        private void PopulatePrice(Card card, JsonElement price)
        {
            foreach (var priceElement in price.EnumerateArray())
            {
                foreach (var priceProperty in priceElement.EnumerateObject())
                {
                    var priceStr = priceProperty.Name;
                    if (priceStrings.Contains(priceStr) && priceProperty.Value.GetString() != "0.00")
                    {
                        card.Price = priceProperty.Value.GetString();
                        return; // Once a non-zero price is found, exit the method
                    }
                }
            }

            // If no valid price is found, set card price to null or any default value
            card.Price = null;
        }

        private T? GetPropertyValue<T>(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property))
            {
                switch (property.ValueKind)
                {
                    case JsonValueKind.String:
                        if (typeof(T) == typeof(string))
                            return (T)(object)property.GetString();
                        break;
                    case JsonValueKind.Number:
                        if (typeof(T) == typeof(int))
                            return (T)(object)property.GetInt32();
                        if (typeof(T) == typeof(double))
                            return (T)(object)property.GetDouble();
                        if (typeof(T) == typeof(long))
                            return (T)(object)property.GetInt64();
                        if (typeof(T) == typeof(float))
                            return (T)(object)property.GetSingle();
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        if (typeof(T) == typeof(bool))
                            return (T)(object)property.GetBoolean();
                        break;
                }
            }
            return default;
        }


        private T GetPropertyValueFromArray<T>(JsonElement element, string arrayName, int index, string propertyName)
        {
            if (element.TryGetProperty(arrayName, out var arrayElement) && arrayElement.ValueKind == JsonValueKind.Array && arrayElement.GetArrayLength() > index)
            {
                var item = arrayElement[index];
                return GetPropertyValue<T>(item, propertyName);
            }

            return default;
        }


        private string GetPropertyArrayValues(JsonElement element, string arrayName)
        {
            if (element.TryGetProperty(arrayName, out var arrayElement) && arrayElement.ValueKind == JsonValueKind.Array)
            {
                var values = new List<string>();
                foreach (var item in arrayElement.EnumerateArray())
                {
                    values.Add(item.GetString());
                }
                return string.Join(",", values);
            }

            return string.Empty;
        }


        private Card ProcessSingleCard(JsonElement card)
        {
            var type = GetPropertyValue<string>(card, "type");
            var frameType = GetPropertyValue<string>(card, "frameType");
            if (string.Equals(type, "Spell Card", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(type, "Trap Card", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(type, "Skill Card", StringComparison.OrdinalIgnoreCase))
            {
                var spellAndTrapCard = new SpellAndTrapCard
                {
                    CardId = GetPropertyValue<int>(card, "id"),
                    Name = GetPropertyValue<string>(card, "name"),
                    Type = type,
                    FrameType = frameType,
                    Description = GetPropertyValue<string>(card, "desc"),
                    Race = GetPropertyValue<string>(card, "race"),
                    Attribute = frameType == "spell" ? "SPELL" : frameType == "trap" ? "TRAP" : null,
                    Archetype = GetPropertyValue<string>(card, "archetype"),
                    YgoProDeckUrl = GetPropertyValue<string>(card, "ygoprodeck_url"),
                    ImageUrl = GetPropertyValueFromArray<string>(card, "card_images", 0, "image_url")
                };

                if (card.TryGetProperty("card_prices", out var price))
                {
                    PopulatePrice(spellAndTrapCard, price);
                }

                return spellAndTrapCard;
            }
            else
            {
                var monsterCard = new MonsterCard
                {
                    CardId = GetPropertyValue<int>(card, "id"),
                    Name = GetPropertyValue<string>(card, "name"),
                    Type = type,
                    FrameType = frameType,
                    Description = GetPropertyValue<string>(card, "desc"),
                    Race = GetPropertyValue<string>(card, "race"),
                    Archetype = GetPropertyValue<string>(card, "archetype"),
                    YgoProDeckUrl = GetPropertyValue<string>(card, "ygoprodeck_url"),
                    ImageUrl = GetPropertyValueFromArray<string>(card, "card_images", 0, "image_url"),
                    Attack = GetPropertyValue<int>(card, "atk"),
                    Attribute = GetPropertyValue<string>(card, "attribute")
                };

                if (frameType == "link")
                {
                    monsterCard.LinkValue = GetPropertyValue<int>(card, "linkval");
                    monsterCard.LinkMarkers = GetPropertyArrayValues(card, "linkmarkers");
                }
                else
                {
                    monsterCard.Defense = GetPropertyValue<int>(card, "def");
                    monsterCard.Level = GetPropertyValue<int>(card, "level");
                }

                if (frameType.Contains("pendulum"))
                {
                    monsterCard.Scale = GetPropertyValue<int>(card, "scale");
                }

                if (card.TryGetProperty("card_prices", out var price))
                {
                    PopulatePrice(monsterCard, price);
                }

                return monsterCard;
            }
        }
    }
}
