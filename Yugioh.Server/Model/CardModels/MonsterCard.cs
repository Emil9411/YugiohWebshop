namespace Yugioh.Server.Model.CardModels
{
    public class MonsterCard : Card
    {
        public int Attack { get; set; }
        public int? Defense { get; set; }
        public int? Level { get; set; }
        public int? Scale { get; set; }
        public int? LinkValue { get; set; }
        public string? LinkMarkers { get; set; }
    }
}
