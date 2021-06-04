using System.Threading.Tasks;

namespace MiniRPG.Navigation
{
    public static class INavigatorExtensions
    {
        public static async Task<bool> ShowPage<T>(this INavigator navigator, INavigationData loadData = null) where T : INavigationPage
        {
            return await navigator.ShowPage(Navigator.GetPageNameByType<T>(), loadData);
        }
    }
}