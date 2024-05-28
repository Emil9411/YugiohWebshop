namespace Yugioh.Server.Model.CardModels
{
    public class CardDto
    {
        public int CardId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Indicate the type of card (Monster, Spell/Trap, etc.)
    }

}
