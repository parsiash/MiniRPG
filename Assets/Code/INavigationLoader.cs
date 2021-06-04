using System.Threading.Tasks;

namespace MiniRPG
{
    public interface INavigationLoader
    {
        Task<bool> LoadHeroSelectionMenu();
        Task<bool> StartBattle();
    }
}
