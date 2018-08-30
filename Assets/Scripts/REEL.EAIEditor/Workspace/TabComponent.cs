using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class TabComponent : MonoBehaviour
	{
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite unselectedSprite;
        [SerializeField] private Text projectNameText;

        [SerializeField] private int tabID = -1;
        private Image tabImage;

        [SerializeField] private TabManager tabManager;
        [SerializeField] private bool isSelected = false;
        [SerializeField] private string tabName = string.Empty;

        private void Awake()
        {
            tabImage = GetComponent<Image>();
            projectNameText = GetComponentInChildren<Text>(true);
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

        public void ReturnToPool(string itemName, Transform parent = null)
        {
            ChangeState(false);
            SetTabName(string.Empty);
            SetTabID(-1);
            ObjectPool.Instance.PushToPool(itemName, gameObject);
        }

        // 탭 이름(프로젝트 이름) 설정.
        public void SetTabName(string tabName)
        {
            this.tabName = tabName;
            if (projectNameText) projectNameText.text = tabName;
        }

        public string GetTabName {  get { return tabName; } }

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