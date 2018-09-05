using UnityEngine;

namespace REEL.EAIEditor
{
    public class TabComponent : MonoBehaviour
    {
        [SerializeField] private TabUI tabUI;
        [SerializeField] private TabData tabData;

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

        // Getter Properties.
        public TabUI GetTabUI { get { return tabUI; } }
        public TabData GetTabData { get { return tabData; } }
    }
}