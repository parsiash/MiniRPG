using System.Threading.Tasks;

namespace MiniRPG.Navigation
{
    /// <summary>
    /// The abstraction of the pages managed in the Navigation System. (INavigator, etc.)
    /// </summary>
    public interface INavigationPage
    {
        string Name { get; }
        Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data);
        Task<bool> OnHide();
        void SetVisible(bool visible);
    }

    /// <summary>
    /// The abstraction of the load data that is provided to an INavigationPage in OnLoaded method.
    /// </summary>
    public interface INavigationData
    {

    }
}