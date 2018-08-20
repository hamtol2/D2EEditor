using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class KeyInputManager : Singleton<KeyInputManager>
    {
        public bool shouldMultiSelect = false;

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A))
            {
                BlockDiagramManager.Instance.SetAllSelected();
            }

#else
            if (shouldMultiSelect && Input.GetKeyDown(KeyCode.A))
            {
                BlockDiagramManager.Instance.SetAllSelected();
            }
#endif

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                BlockDiagramManager.Instance.DeleteSelected();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                shouldMultiSelect = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                shouldMultiSelect = false;
            }
        }
    }
}