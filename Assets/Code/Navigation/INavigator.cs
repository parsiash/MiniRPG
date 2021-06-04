using System.Threading.Tasks;

namespace MiniRPG.Navigation
{
    /// <summary>
    /// The interface of all the navigators.
    /// Navigator is responsible for Showing/Managing Navigation Pages.
    /// Currently only one RootNavigator is available (Initialized in the ServiceInitializer)
    /// But in future it can be extended to a more comprehensive navigation system. (E.g. Adding Back stack support, etc)
    /// </summary>
    public interface INavigator
    {
        INavigationPage GetPage(string name);
        void AddPage(INavigationPage page);
        Task<bool> ShowPage(string name, INavigationData loadData = null);
    }
}