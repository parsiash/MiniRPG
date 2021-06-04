namespace MiniRPG.UI
{
    public class HeroInfo
    {
        public HeroInfoAttribute[] attributes { get; private set; }
        public int AttributesCount => attributes.Length;

        public HeroInfo(params HeroInfoAttribute[] attributes)
        {
            this.attributes = attributes;
        }

        public HeroInfoAttribute GetAttribute(int index)
        {
            if(index >= 0 && index <= attributes.Length)
            {
                return attributes[index];
            }

            return null;
        }
    }
}