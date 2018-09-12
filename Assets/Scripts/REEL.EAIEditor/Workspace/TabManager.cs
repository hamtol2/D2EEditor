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

        [SerializeField] private int selectedTabIndex = 0;
        [SerializeField] private int prevSelectedTabIndex = 0;
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

            ChangeTabState(currentTabs.Count - 1);
            RearrangeTabsPosition();
        }

        public void RemoveTab(TabComponent tab)
        {
            TabComponent foundTab = GetTab(tab.TabID);

            if (foundTab != null)
            {
                currentTabs.RemoveAt(foundTab.TabID);

                foundTab.ChangeState(false);

                // Retun to object pool.
                foundTab.ReturnToPool(tabItemName, transform);
            }

            RearrangeTabsPosition(true);
        }

        private void SetAllTabUnselected()
        {
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                currentTabs[ix].ChangeState(false);
            }

            WorkspaceManager.Instance.SetAllUnselected();
        }

        public void ChangeTabState(int tabID, bool hasRemoved = false)
        {
            if (!hasRemoved && prevSelectedTabIndex.Equals(tabID)) return;

            if (prevSelectedTabIndex != -1 && prevSelectedTabIndex < currentTabs.Count)
            {
                currentTabs[prevSelectedTabIndex].GetTabData.SaveState();
                SetAllTabUnselected();
            }

            selectedTabIndex = tabID;
            currentTabs[selectedTabIndex].ChangeState(true);

            prevSelectedTabIndex = selectedTabIndex;
        }

        public void SaveProject(string projectName)
        {
            if (selectedTabIndex == -1) return;

            projectName = string.IsNullOrEmpty(projectName) ? currentTabs[selectedTabIndex].GetTabUI.GetTabName : projectName;
            ProjectFormat projectSaveFormatData = WorkspaceManager.Instance.GetSaveFormat(projectName);
            currentTabs[selectedTabIndex].GetTabData.SaveProject(projectSaveFormatData);

            WorkspaceManager.Instance.CompileToXML(projectSaveFormatData);
        }

        private void UpdateCurrentProject(string projectName)
        {
            currentTabs[selectedTabIndex].GetTabData.LoadProject(projectName);
        }

        private TabComponent GetTab(int tabID)
        {
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                if (currentTabs[ix].TabID.Equals(tabID)) return currentTabs[ix];
            }

            return null;
        }

        private TabComponent LoadTab(string tabName)
        {
            TabComponent newTab = GetTabComponentFromOjbectPool();

            newTab.GetTabUI.SetTabName(tabName);
            newTab.TabID = currentTabs.Count;
            newTab.SetManager(this);

            newTab.GetTabData.LoadProject(tabName);

            return newTab;
        }

        private TabComponent CreateNewTab(string tabName)
        {
            TabComponent newTab = GetTabComponentFromOjbectPool();

            bool isTabNameNull = string.IsNullOrEmpty(tabName);
            string projectName = isTabNameNull ? "Test" + (currentTabs.Count + 1).ToString() : tabName;

            // Set TabUI.
            newTab.GetTabUI.SetTabName(projectName);
            newTab.GetTabData.CreateNewProject(projectName);
            newTab.SetManager(this);

            // Test. Add Entry Block.
            WorkspaceManager.Instance.AddEntryNode();

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

        private void RearrangeTabsPosition(bool hasRemoved = false)
        {
            if (currentTabs.Count == 0)
            {
                newTabComponent.anchoredPosition = Vector2.zero;
                selectedTabIndex = prevSelectedTabIndex = -1;
                return;
            }

            bool anyTabSelected = false;
            Vector2 newPos = Vector2.zero;
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                newPos.x = ix * currentTabs[ix].GetTabUI.GetTabSize().x + (ix == 0 ? 0 : ix) * tabOffset;
                newPos.y = 0f;
                currentTabs[ix].GetComponent<RectTransform>().anchoredPosition = newPos;
                currentTabs[ix].TabID = ix;

                if (currentTabs[ix].GetTabUI.GetSelectedState)
                {
                    anyTabSelected = true;
                    selectedTabIndex = ix;
                }
            }

            newTabComponent.anchoredPosition = new Vector2(newPos.x + currentTabs[0].GetTabUI.GetTabSize().x + tabOffset, 0f);

            if (currentTabs.Count == 1 || !anyTabSelected) ChangeTabState(0, hasRemoved);
        }

        // Properties.
        public TabComponent GetCurrentTab
        {
            get
            {
                if (currentTabs.Count == 0 || selectedTabIndex == -1) return null;
                return currentTabs[selectedTabIndex];
            }
        }
        public string GetCurrentProjectName { get { return GetCurrentTab.GetTabData.GetProjectData.GetProjectFormat.projectName; } }
        public bool CanAddTab { get { return currentTabs.Count < maxTabCount; } }
    }
}