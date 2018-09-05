using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class TabManager : MonoBehaviour
	{
        public enum TabAddType { New, Load }

        [SerializeField] public List<TabComponent> currentTabs = new List<TabComponent>();
        [SerializeField] private GameObject tabPrefab = null;
        [SerializeField] private float tabOffset = 15f;
        [SerializeField] private RectTransform newTabComponent = null;

        [SerializeField] private int selectedTabIndex = -1;
        [SerializeField] private int maxTabCount = 8;
        [SerializeField] private string tabItemName = "tab";

        public void CreateTab(string tabName)
        {
            AddTab(tabName, TabAddType.New);
        }

        public void AddTab(string tabName, TabAddType addType)
        {
            if (!CanAddTab) return;

            switch (addType)
            {
                case TabAddType.New: currentTabs.Add(CreateNewTab(tabName)); break;
                case TabAddType.Load: currentTabs.Add(LoadTab(tabName)); break;
                default: break;
            }
            
            RearrangeTabsPosition();
        }

        public void RemoveTab(TabComponent tab)
        {
            TabComponent foundTab = GetTab(tab.GetTabID);
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
                currentTabs[ix].GetTabUI.ChangeState(false);
            }
        }

        public void ChangeTabState(int tabID)
        {
            SetAllTabUnselected();
            selectedTabIndex = tabID;
            currentTabs[selectedTabIndex].GetTabUI.ChangeState(true);
        }

        public void SaveProject(string projectName)
        {
            if (selectedTabIndex == -1) return;

            projectName = string.IsNullOrEmpty(projectName) ? currentTabs[selectedTabIndex].GetTabUI.GetTabName : projectName;
            WorkspaceManager.Instance.SaveToFile(projectName);
        }

        private TabComponent GetTab(int tabID)
        {
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                if (currentTabs[ix].GetTabID.Equals(tabID)) return currentTabs[ix];
            }

            return null;
        }

        private TabComponent LoadTab(string tabName)
        {
            TabComponent newTab = GetTabComponentFromOjbectPool();

            newTab.GetTabUI.SetTabName(tabName);
            newTab.SetManager(this);

            newTab.GetTabData.LoadProject(tabName);

            // Test..
            ProjectFormat project = newTab.GetTabData.GetProjectData.GetProjectFormat;
            WorkspaceManager.Instance.LoadFromProjectFormat(project);

            return newTab;
        }

        private TabComponent CreateNewTab(string tabName)
        {
            TabComponent newTab = GetTabComponentFromOjbectPool();

            bool isTabNameNull = string.IsNullOrEmpty(tabName);

            // Set TabUI.
            newTab.GetTabUI.SetTabName(isTabNameNull ? "Test" + (currentTabs.Count + 1).ToString() : tabName);
            newTab.SetManager(this);

            return newTab;
        }

        private TabComponent GetTabComponentFromOjbectPool()
        {
            // Get Object from object pool.
            GameObject newTabObj = ObjectPool.Instance.PopFromPool(tabItemName, transform);
            newTabObj.transform.position = Vector3.zero;
            newTabObj.transform.localScale = Vector3.one;
            newTabObj.SetActive(true);

            return newTabObj.GetComponent<TabComponent>();
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
                newPos.x = ix * currentTabs[ix].GetTabUI.GetTabSize().x + (ix == 0? 0 : ix) * tabOffset;
                newPos.y = 0f;
                currentTabs[ix].GetComponent<RectTransform>().anchoredPosition = newPos;
                currentTabs[ix].SetTabID(ix);

                if (currentTabs[ix].GetTabUI.GetSelectedState)
                {
                    anyTabSelected = true;
                    selectedTabIndex = ix;
                }
            }

            newTabComponent.anchoredPosition = new Vector2(newPos.x + currentTabs[0].GetTabUI.GetTabSize().x + tabOffset, 0f);

            if (currentTabs.Count == 1 || !anyTabSelected) ChangeTabState(0);
        }

        public bool CanAddTab { get { return currentTabs.Count < maxTabCount; } }
	}
}