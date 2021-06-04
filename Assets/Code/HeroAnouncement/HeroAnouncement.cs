namespace MiniRPG
{
    public class HeroAnouncement
    {
        public int heroId { get; set; }
        public string text { get; set; }

        public HeroAnouncement(int heroId, string text)
        {
            this.heroId = heroId;
            this.text = text;
        }
    }
}
