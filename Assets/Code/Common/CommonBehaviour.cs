using UnityEngine;

namespace MiniRPG.Common
{
    /// <summary>
    /// A Base class for most of the MonoBehaviours in the game.
    /// Common functionalities that might be used on any MonoBehaviour is implemented here.
    /// </summary>
    public class CommonBehaviour : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
