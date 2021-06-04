using System.Collections.Generic;
using MiniRPG.Common;
using MiniRPG.Metagame;
using UnityEngine;
using UnityEngine.UI;

namespace MiniRPG.UI
{
    public class HeroInfoPopup : UIComponent
    {
        private List<HeroInfoAttributeComponent> _attributeComponents;
        private List<HeroInfoAttributeComponent> attributeComponents
        {
            get
            {
                _attributeComponents = _attributeComponents ?? new List<HeroInfoAttributeComponent>(GetComponentsInChildren<HeroInfoAttributeComponent>(true));
                return _attributeComponents;
            }
        }

        [SerializeField] private UIComponent panelRoot;
        [SerializeField] private RectTransform attributeComponentsLayoutRoot;

        private Canvas canvas => RetrieveCachedComponentInChildren<Canvas>();

        public void ShowPopup(HeroInfo heroInfo, Vector2 worldPosition)
        {
            HideAllAttributes();
            SetActive(true);

            for(int i = 0; i < heroInfo.AttributesCount; i++)
            {
                GetAttributeComponent(i).Show(heroInfo.GetAttribute(i));
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(attributeComponentsLayoutRoot);

            panelRoot.SetWorldPosition(worldPosition);
        }

        public void HideAllAttributes()
        {
            foreach(var attributeComponent in attributeComponents)
            {
                attributeComponent.SetActive(false);
            }
        }

        private HeroInfoAttributeComponent GetAttributeComponent(int index)
        {
            for(int i = attributeComponents.Count; i <= index; i++)
            {
                var attributeComponent = GameObject.Instantiate<HeroInfoAttributeComponent>(attributeComponents[0], attributeComponentsLayoutRoot);
                attributeComponent.transform.localScale = Vector3.one;
                attributeComponents.Add(attributeComponent);
            }

            return attributeComponents[index];
        }

        public void OnClick()
        {
            SetActive(false);
        }
    }
}