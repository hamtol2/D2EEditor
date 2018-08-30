using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class TabManager : MonoBehaviour
	{
        [SerializeField] public List<TabComponent> currentTabs = new List<TabComponent>();
        [SerializeField] private GameObject tabPrefab;
        [SerializeField] private float tabOffset = 15f;
        [SerializeField] private RectTransform newTabComponent;

        [SerializeField] private int selectedTabIndex = -1;
        [SerializeField] private int maxTabCount = 8;
        [SerializeField] private string tabItemName = "tab";

        public void AddTab()
        {
            if (currentTabs.Count >= maxTabCount) return;

            currentTabs.Add(CreateNewTab());
            RearrangeTabsPosition();
        }

        public void RemoveTab(TabComponent tab)
        {
            TabComponent foundTab = GetTab(tab.GetTabID);
            if (foundTab != null)
            {
                currentTabs.RemoveAt(foundTab.GetTabID);
                foundTab.ReturnToPool(tabItemName, transform);
                //ObjectPool.Instance.PushToPool(tabItemName, foundTab.gameObject, transform);
                //Destroy(foundTab.gameObject);
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

        private TabComponent GetTab(int tabID)
        {
            for (int ix = 0; ix < currentTabs.Count; ++ix)
            {
                if (currentTabs[ix].GetTabID.Equals(tabID)) return currentTabs[ix];
            }

            return null;
        }

        private TabComponent CreateNewTab()
        {
            GameObject newTabObj = ObjectPool.Instance.PopFromPool(tabItemName, transform);
            newTabObj.transform.position = Vector3.zero;
            newTabObj.transform.localScale = Vector3.one;
            newTabObj.SetActive(true);

            //GameObject newTabObj = Instantiate(tabPrefab) as GameObject;
            //newTabObj.transform.SetParent(transform);
            //newTabObj.transform.position = Vector3.zero;
            //newTabObj.transform.localScale = Vector3.one;

            TabComponent newTab = newTabObj.GetComponent<TabComponent>();
            newTab.SetManager(this);

            return newTab;
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
	}
}