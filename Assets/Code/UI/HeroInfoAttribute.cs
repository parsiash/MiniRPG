namespace MiniRPG.UI
{
    public class HeroInfoAttribute
    {
        public string name { get; private set; }
        public string value { get; private set; }

        public HeroInfoAttribute(string name, object value)
        {
            this.name = name;
            this.value = value.ToString();
        }
    }
}