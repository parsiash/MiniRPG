using System.Collections.Generic;
using System.Linq;
using MiniRPG.Metagame;

namespace MiniRPG
{
    public interface IHeroAnouncementHandler
    {
        IEnumerable<HeroAnouncement> FlushAnouncements();
    }
    
    public class HeroAnouncementHandler : IHeroAnouncementHandler, IProfileUpdateListener
    {
        private List<HeroAnouncement> _anouncements;
        private Common.ILogger _logger;

        public HeroAnouncementHandler(Common.ILogger logger)
        {
            _anouncements = new List<HeroAnouncement>();
            _logger = logger;
        }

        public void OnProfileUpdate(IProfileUpdate update)
        {
            if(update is IncrementHeroLevel)
            {
                var incHeroLevelUpdate = update as IncrementHeroLevel;
                
                AddAnouncement(new HeroAnouncement(
                    incHeroLevelUpdate.heroId, $"+ Attack {incHeroLevelUpdate.attackIncrease} \n + Health {incHeroLevelUpdate.healthIncrease}"
                ));
            }else if(update is IncreaseHeroXP)
            {
                var increaseXPUpdate = update as IncreaseHeroXP;

                AddAnouncement(new HeroAnouncement(
                    increaseXPUpdate.heroId, $"+ XP {increaseXPUpdate.amount}"
                ));
            }else if(update is AddHero)
            {
                var addHeroUpdate = update as AddHero;

                AddAnouncement(new HeroAnouncement(
                    addHeroUpdate.hero.heroId, "New Hero"
                ));
            }
        }

        private void AddAnouncement(HeroAnouncement anouncement)
        {
            _anouncements.RemoveAll(item => item.heroId == anouncement.heroId);
            _anouncements.Add(anouncement);
        }

        public IEnumerable<HeroAnouncement> FlushAnouncements()
        {
            var anouncements = _anouncements.ToArray();
            _anouncements = new List<HeroAnouncement>();

            return anouncements;
        }
    }
}
