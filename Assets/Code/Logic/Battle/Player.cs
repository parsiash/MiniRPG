using System.Collections.Generic;
using System.Linq;

namespace MiniRPG.Logic.Battle
{
    public class Player
    {
        public int index { get; private set; }
        public List<Unit> units { get; private set; }

        public Unit[] AliveUnits => units.Where(u => !u.IsDead).ToArray();
        
        public int GetUnitIndexById(int id)
        {
            return units.FindIndex(u => u.id == id);
        }

        public Player(int index)
        {
            this.index = index;
            units = new List<Unit>();
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(int id)
        {
            units.RemoveAll(u => u.id == id);
        }

        public PlayerBattleResult GetResult()
        {
            return new PlayerBattleResult(
                index,
                units.Select(u => u.GetResult()).ToArray()
            );
        }
    }
}