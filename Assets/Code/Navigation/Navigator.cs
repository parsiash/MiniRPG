using System.Collections.Generic;
using System.Threading.Tasks;
using MiniRPG.Common;

namespace MiniRPG.Navigation
{
    public interface INavigator
    {
        INavigationPage GetPage(string name);
        void AddPage(INavigationPage page);
        Task<bool> ShowPage(string name, INavigationData loadData = null);
    }

    public class Navigator : INavigator
    {
        private Dictionary<string, INavigationPage> _pages;
        private ILogger _logger;
        private INavigationPage _currentPage;

        public Navigator(ILogger logger)
        {
            _pages = new Dictionary<string, INavigationPage>();
            _logger = logger;
        }

        public void AddPage(INavigationPage page)
        {
            if(_pages.ContainsKey(page.Name))
            {
                _logger.LogError($"Cannot add the navigation page : {page.Name}. A page with the same name exists in this navigator");
                return;
            }

            _pages[page.Name] = page;
        }

        public INavigationPage GetPage(string name)
        {
            if(_pages.TryGetValue(name, out var page))
            {
                return page;
            }

            return null;
        }

        public async Task<bool> ShowPage(string name, INavigationData loadData = null)
        {
            var page = GetPage(name);
            if(page == null)
            {
                _logger.LogError($"No page found with name : {name}");
                return false;
            }

            _currentPage = page;

            page.SetVisible(true);

            try
            {
                var success = await page.OnLoaded(this, loadData);
                if(success)
                {
                    HideCurrentPage();
                }

                return success;

            }catch(NavigationException exp)
            {
                _logger.LogError($"Exception occured while loading page {name} : {exp.Message} \n {exp.StackTrace}");
                return false;
            }
        }

        private void HideCurrentPage()
        {
            if (_currentPage != null)
            {
                _currentPage.SetVisible(false);
                _currentPage = null;
            }
        }
    }
}