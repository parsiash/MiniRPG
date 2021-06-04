namespace MiniRPG.UI
{
    public class HeroInfoAttributeComponent : UIComponent
    {
        private CustomText text => RetrieveCachedComponentInChildren<CustomText>();

        public void Show(HeroInfoAttribute attribute)
        {
            SetActive(true);
            text.text = attribute.name + " : " + attribute.value;
        }
    }
}