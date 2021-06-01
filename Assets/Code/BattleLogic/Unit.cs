using MiniRPG.Common;

namespace MiniRPG.BattleLogic
{
    public class Unit : Entity
    {
        public int index { get; private set; }
        public int level { get; private set; }
        public int experience { get; private set; }
        private Player _player;

        public Unit(string name, int id, BattleSimulation battleSimulation, ILogger logger) : base(name, id, battleSimulation, logger)
        {
        }

        public Player player => _player;
        public int PlayerIndex => player != null ? player.index : -1;

        public AttackComponent attackComponent => GetComponent<AttackComponent>();
        public HealthComponent healthComponent => GetComponent<HealthComponent>();
        public bool IsDead => healthComponent.IsDead;

        public void Init(Player player)
        {
            _player = player;
        }
    }
}