namespace Yugioh.Server.Model
{
    public class MonsterCard : Card
    {
        public int Attack { get; set; }
        public int? Defense { get; set; }
        public int? Level { get; set; }
        public string? Attribute { get; set; }
        public int? Scale { get; set; }
        public int? LinkValue { get; set; }
        public string? LinkMarkers { get; set; }
    }
}
