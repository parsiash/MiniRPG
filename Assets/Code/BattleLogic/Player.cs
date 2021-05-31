using System.Collections.Generic;

namespace MiniRPG.BattleLogic
{
    public class Player
    {
        public int index { get; private set; }
        public List<Unit> units { get; private set; }

        public Player(int index, List<Unit> units)
        {
            this.index = index;
            this.units = units;
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(int id)
        {
            units.RemoveAll(u => u.id == id);
        }
    }
}