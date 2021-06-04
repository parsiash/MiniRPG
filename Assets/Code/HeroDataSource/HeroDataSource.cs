using MiniRPG.Logic.Battle;
using MiniRPG.Common;
using MiniRPG.Logic.Metagame;
using UnityEngine;
using MiniRPG.Logic;

namespace MiniRPG
{
    /// <summary>
    /// It is used to generate heroes.
    /// </summary>
    public interface IHeroDataSource
    {
        HeroData GetRandomHero(int heroId);
        HeroData GetRandomEnemy(int level);
    }
    
    /// <summary>
    /// A random hero generator implementation of IHeroDataSource.
    /// It generates heroes and enemies based on HeroTemplates in Resources folder.
    /// It can be later replaced by any other implementation, E.g. loading heroes from config files which can be updated from the server.
    /// </summary>
    public class HeroDataSource : IHeroDataSource
    {
        private IUnitStatProvider unitStatProvider;
        private HeroTemplatesAsset heroTemplatesAsset;
        private Common.ILogger logger;

        public HeroDataSource(IUnitStatProvider unitStatProvider, HeroTemplatesAsset heroTemplatesAsset, Common.ILogger logger)
        {
            this.unitStatProvider = unitStatProvider;
            this.heroTemplatesAsset = heroTemplatesAsset;
            this.logger = logger;
        }

        private UnitStat GenerateBaseStat()
        {
            return new UnitStat
            (
                Random.Range(5, 10),
                Random.Range(20, 30)
            );
        }

        public HeroData GetRandomEnemy(int level)
        {
            UnitStat baseStat = GenerateBaseStat();
            UnitStat currentStat = unitStatProvider.GetUnitStatByLevel(baseStat, level);
            
            return new HeroData(
                -1,
                "Enemy",
                level,
                0,
                baseStat,
                currentStat,
                MyColor.FromUnityColor(Color.red),
                3
            );
        }
        public HeroData GetRandomHero(int heroId)
        {
            var heroTemplate = heroTemplatesAsset.GetHeroTemplate(heroId);
            UnitStat baseStat = GenerateBaseStat();

            return new HeroData(
                heroId,
                heroTemplate != null ? heroTemplate.name : "Hero_" + heroId,
                1,
                0,
                baseStat,
                baseStat,
                MyColor.FromUnityColor(heroTemplate != null ? heroTemplate.color : Random.ColorHSV()),
                1
            );
        }
    }
}
