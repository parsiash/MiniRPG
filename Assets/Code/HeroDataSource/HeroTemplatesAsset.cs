using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG
{
    [CreateAssetMenu(fileName = "HeroTemplates", menuName = "Custom/HeroTemplates", order = 1)]
    public class HeroTemplatesAsset: ScriptableObject
    {
        private static HeroTemplatesAsset instance;

        public static HeroTemplatesAsset Instance
        {
            get
            {
                if (!instance)
                {
                    instance = Resources.Load<HeroTemplatesAsset>("HeroTemplates");
                    if(!instance)
                    {
                        DefaultLogger.Instance.LogError("HeroTemplates asset does not exist in Resources folder");
                    }
                }

                return instance;
            }

        }

        [SerializeField]
        private HeroTemplate[] templates;

        public HeroTemplate GetHeroTemplate(int index)
        {
            if(index < 0)
            {
                return null;
            }

            return templates[index % templates.Length];
        }
    }
}
