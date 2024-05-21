namespace Yugioh.Server.Model
{
    public abstract class Card
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? FrameType { get; set; }
        public string? Race { get; set; }
        public string? Archetype { get; set; }
        public string? Description { get; set; }
        public string? YgoProDeckUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? Price { get; set; }
    }
}
