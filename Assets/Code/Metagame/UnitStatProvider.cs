using System;
using MiniRPG.BattleLogic;

namespace MiniRPG.Metagame
{
    public interface IUnitStatProvider
    {
        UnitStat GetUnitStatByLevel(UnitStat baseStat, int level);
    }

    public class UnitStatProvider : IUnitStatProvider
    {
        
        private int GetAttribute(int baseAttribute, int level)
        {
            var factor = Math.Pow(1.1f, level);
            return (int)(factor * baseAttribute);
        }

        public UnitStat GetUnitStatByLevel(UnitStat baseStat, int level)
        {
            return new UnitStat(
                GetAttribute(baseStat.attack, level),
                GetAttribute(baseStat.health, level)
            );
        }
    }
} 