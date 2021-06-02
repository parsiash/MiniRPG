using System.Threading.Tasks;
using MiniRPG.Common;

namespace MiniRPG.Navigation
{
    public class NavigationPageBase : CommonBehaviour, INavigationPage
    {
        public virtual string Name => Navigator.GetPageNameByType(GetType());
        public INavigator parentNavigator { get; protected set; }
        
        public virtual async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            this.parentNavigator = parentNavigator;
            return true;
        }

        public virtual void SetVisible(bool visible)
        {
            SetActive(visible);
        }
    }
}