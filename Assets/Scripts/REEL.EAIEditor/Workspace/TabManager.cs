using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class TabManager : MonoBehaviour
	{
        [SerializeField] public List<TabUI> currentTabs = new List<TabUI>();
        [SerializeField] private GameObject tabPrefab;
        [SerializeField] private float tabOffset = 15f;
        [SerializeField] private RectTransform newTabComponent;

        [SerializeField] private int selectedTabIndex = -1;
        [SerializeField] private int maxTabCount = 8;
        [SerializeField] private string tabItemName = "tab";

        //public void AddTab()
        //{
        //    if (!CanAddTab) return;

        //    currentTabs.Add(CreateNewTab());
        //    RearrangeTabsPosition();
        //}

        public void AddTab(string tabName)
        {
            if (!CanAddTab) return;

            currentTabs.Add(CreateNewTab(tabName));
            RearrangeTabsPosition();
        }

        public void RemoveTab(TabUI tab)
        {
            TabUI foundTab = GetTab(tab.GetTabID);
            if (foundTab != null)
            {
                currentTabs.RemoveAt(foundTab.GetTabID);

                // Retun to object pool.
                foundTab.ReturnToPool(tabItemName, transform);
            }

            RearrangeTabsPosition();
        }

        private void SetAllTabUnselected()
        {
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                currentTabs[ix].ChangeState(false);
            }
        }

        public void ChangeTabState(int tabID)
        {
            SetAllTabUnselected();
            selectedTabIndex = tabID;
            currentTabs[selectedTabIndex].ChangeState(true);
        }

        public void SaveProject(string projectName)
        {
            if (selectedTabIndex == -1) return;

            projectName = string.IsNullOrEmpty(projectName) ? currentTabs[selectedTabIndex].GetTabName : projectName;
            BlockDiagramManager.Instance.SaveToFile(projectName);

            //BlockDiagramManager.Instance.SaveToFile(currentTabs[selectedTabIndex].GetTabName);
        }

        private TabUI GetTab(int tabID)
        {
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                if (currentTabs[ix].GetTabID.Equals(tabID)) return currentTabs[ix];
            }

            return null;
        }

        //private TabComponent CreateNewTab()
        //{
        //    // Get Object from object pool.
        //    TabComponent newTab = CreateTabComponent();
        //    newTab.SetTabName("Test" + (currentTabs.Count + 1).ToString());
        //    newTab.SetManager(this);

        //    return newTab;
        //}

        private TabUI CreateNewTab(string tabName)
        {
            TabUI newTab = CreateTabComponent();
            newTab.SetTabName(string.IsNullOrEmpty(tabName) ? "Test" + (currentTabs.Count + 1).ToString() : tabName);
            newTab.SetManager(this);

            return newTab;
        }

        private TabUI CreateTabComponent()
        {
            // Get Object from object pool.
            GameObject newTabObj = ObjectPool.Instance.PopFromPool(tabItemName, transform);
            newTabObj.transform.position = Vector3.zero;
            newTabObj.transform.localScale = Vector3.one;
            newTabObj.SetActive(true);

            return newTabObj.GetComponent<TabUI>();
        }

        private void RearrangeTabsPosition()
        {
            if (currentTabs.Count == 0)
            {
                newTabComponent.anchoredPosition = Vector2.zero;
                return;
            }

            bool anyTabSelected = false;
            Vector2 newPos = Vector2.zero;
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {   
                newPos.x = ix * currentTabs[ix].GetTabSize().x + (ix == 0? 0 : ix) * tabOffset;
                newPos.y = 0f;
                currentTabs[ix].GetComponent<RectTransform>().anchoredPosition = newPos;
                currentTabs[ix].SetTabID(ix);

                if (currentTabs[ix].GetSelectedState)
                {
                    anyTabSelected = true;
                    selectedTabIndex = ix;
                }
            }

            newTabComponent.anchoredPosition = new Vector2(newPos.x + currentTabs[0].GetTabSize().x + tabOffset, 0f);

            if (currentTabs.Count == 1 || !anyTabSelected) ChangeTabState(0);
        }

        public bool CanAddTab { get { return currentTabs.Count < maxTabCount; } }
	}
}