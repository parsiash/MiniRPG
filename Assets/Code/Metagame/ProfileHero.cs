namespace MiniRPG.Metagame
{
    public class ProfileHero
    {
        public int heroId { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int attack { get; set; }
        public int health { get; set; }

        public ProfileHero(int heroId, string name, int level, int experience, int attack, int health)
        {
            this.heroId = heroId;
            this.name = name;
            this.level = level;
            this.experience = experience;
            this.attack = attack;
            this.health = health;
        }
    }
}