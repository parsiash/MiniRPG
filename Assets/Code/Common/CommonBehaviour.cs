using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniRPG.Common
{
    /// <summary>
    /// A Base class for most of the MonoBehaviours in the game.
    /// Common functionalities that might be used on any MonoBehaviour is implemented here.
    /// </summary>
    public class CommonBehaviour : MonoBehaviour
    {
         private Dictionary<string, Component> _cachedComoponents;
        private Dictionary<string, Component> cachedComoponents
        {
            get
            {
                _cachedComoponents = _cachedComoponents ?? new Dictionary<string, Component>();
                return _cachedComoponents;
            }
        }

        private Dictionary<string, Component> _cachedComoponentsInChildren;
        private Dictionary<string, Component> cachedComoponentsInChildren
        {
            get
            {
                _cachedComoponentsInChildren = _cachedComoponentsInChildren ?? new Dictionary<string, Component>();
                return _cachedComoponentsInChildren;
            }
        }
        
        protected ILogger logger => DefaultLogger.Instance;

          private string GetComponentTypeName<T>() where T : Component
        {
            return GetComponentTypeName(typeof(T));
        }

        private string GetComponentTypeName(Type type)
        {
            return type.Name;
        }

        public T RetrieveCachedComponent<T>() where T : Component
        {
            var name = GetComponentTypeName<T>();

            if(cachedComoponents.TryGetValue(name, out var component))
            {
                return component as T;
            }

            component = GetComponent<T>();
            if(component)
            {
                cachedComoponents[name] = component;
            }

            return component as T;
        }        

        public T RetrieveCachedComponentInChildren<T>(bool includeInActive = true) where T : Component
        {
            var name = GetComponentTypeName<T>();

            if(cachedComoponentsInChildren.TryGetValue(name, out var component))
            {
                return component as T;
            }

            component = GetComponentInChildren<T>(includeInActive);
            if(component)
            {
                cachedComoponentsInChildren[name] = component;
            }

            return component as T;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
