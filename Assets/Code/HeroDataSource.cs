using MiniRPG.Common;
using MiniRPG.Metagame;
using UnityEngine;

namespace MiniRPG
{
    public interface IHeroDataSource
    {
        HeroData GetRandomHero(int heroId);
        HeroData GetRandomEnemy(int level);
    }

    public class HeroDataSource : IHeroDataSource
    {
        private HeroTemplatesAsset heroTemplatesAsset;
        private Common.ILogger logger;

        public HeroDataSource(HeroTemplatesAsset heroTemplatesAsset, Common.ILogger logger)
        {
            this.heroTemplatesAsset = heroTemplatesAsset;
            this.logger = logger;
        }

        public HeroData GetRandomEnemy(int level)
        {
            return new HeroData(
                -1,
                "Enemy",
                Random.Range(1, 20),
                Random.Range(1, 20),
                Random.Range(10, 15),
                Random.Range(20, 30),
                MyColor.FromUnityColor(Color.red),
                3
            );
        }
        public HeroData GetRandomHero(int heroId)
        {
            var heroTemplate = heroTemplatesAsset.GetHeroTemplate(heroId);

            return new HeroData(
                heroId,
                heroTemplate != null ? heroTemplate.name : "Hero_" + heroId,
                Random.Range(1, 20),
                Random.Range(1, 20),
                Random.Range(10, 15),
                Random.Range(20, 30),
                MyColor.FromUnityColor(heroTemplate != null ? heroTemplate.color : Random.ColorHSV()),
                1
            );
        }
    }
}
