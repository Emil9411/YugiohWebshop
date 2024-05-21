namespace Yugioh.Server.Model
{
    public class AllCardResponse
    {
        public List<MonsterCard>? MonsterCards { get; set; } = new List<MonsterCard>();
        public List<SpellAndTrapCard>? SpellAndTrapCards { get; set; } = new List<SpellAndTrapCard>();
        public bool Error { get; set; }
    }
}
