using System.Threading.Tasks;

namespace MiniRPG.Navigation
{
    public interface INavigationPage
    {
        string Name { get; }
        Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data);
        Task<bool> OnHide();
        void SetVisible(bool visible);
    }

    public interface INavigationData
    {

    }
}