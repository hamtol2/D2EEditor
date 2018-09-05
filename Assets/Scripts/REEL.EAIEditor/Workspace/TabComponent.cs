using UnityEngine;

namespace REEL.EAIEditor
{
    public class TabComponent : MonoBehaviour
    {
        [SerializeField] private TabUI tabUI;
        [SerializeField] private TabData tabData;

        [SerializeField] private TabManager tabManager;
        [SerializeField] private int tabID = -1;

        private void Awake()
        {
            Initialize();
        }

        // Init components.
        private void Initialize()
        {
            if (!tabUI) tabUI = GetComponent<TabUI>();
            if (!tabData) tabData = GetComponent<TabData>();
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

        public void ReturnToPool(string itemName, Transform parent = null)
        {
            GetTabUI.ChangeState(false);
            GetTabUI.SetTabName(string.Empty);
            SetTabID(-1);
            ObjectPool.Instance.PushToPool(itemName, gameObject);
        }

        // Getter Properties.
        public TabUI GetTabUI { get { return tabUI; } }
        public TabData GetTabData { get { return tabData; } }
        public int GetTabID { get { return tabID; } }

        public void SetTabID(int id)
        {
            tabID = id;
        }
    }
}