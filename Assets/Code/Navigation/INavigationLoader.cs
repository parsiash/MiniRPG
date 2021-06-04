using System.Threading.Tasks;

namespace MiniRPG
{
    /// <summary>
    /// A helper navigation loader for reusing common navigation boilerplate code.
    /// </summary>
    public interface INavigationLoader
    {
        Task<bool> LoadHeroSelectionMenu();
        Task<bool> StartBattle();
    }
}
