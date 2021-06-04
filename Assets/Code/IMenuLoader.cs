using System.Threading.Tasks;

namespace MiniRPG
{
    public interface IMenuLoader
    {
        Task<bool> LoadHeroSelectionMenu();
    }
}
