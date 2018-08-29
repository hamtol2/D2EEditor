using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.Test
{
	public class TabComponent : MonoBehaviour
	{
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite unselectedSprite;

        [SerializeField] private int tabID = -1;
        private Image tabImage;

        [SerializeField] private TabManager tabManager;
        [SerializeField] private bool isSelected = false;

        private void Awake()
        {
            tabImage = GetComponent<Image>();
            ChangeState(false);
        }

        public void CloseTab()
        {
            if (tabManager) tabManager.RemoveTab(this);
        }

        public void SetManager(TabManager tabManager)
        {
            this.tabManager = tabManager;
        }

        public void OnTabClicked()
        {
            if (tabManager) tabManager.ChangeTabState(tabID);
        }

        public void ChangeState(bool isSelected)
        {
            if (isSelected) tabImage.sprite = selectedSprite;
            else tabImage.sprite = unselectedSprite;

            this.isSelected = isSelected;
        }

        public Vector2 GetTabSize()
        {
            return tabImage.GetComponent<RectTransform>().sizeDelta;
        }

        public int GetTabID { get { return tabID; } }

        public void SetTabID(int id)
        {
            tabID = id;
        }

        public bool GetSelectedState {  get { return isSelected; } }
    }
}