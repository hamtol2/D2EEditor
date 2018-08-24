using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class KeyInputManager : Singleton<KeyInputManager>
    {
        public bool shouldMultiSelect = false;
        public bool isShiftPressed = false;
        public bool isAltPressed = false;

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A)) BlockDiagramManager.Instance.SetAllSelected();

#else
            if (shouldMultiSelect && Input.GetKeyDown(KeyCode.A)) BlockDiagramManager.Instance.SetAllSelected();
#endif

            if (Input.GetKeyDown(KeyCode.Delete)) BlockDiagramManager.Instance.DeleteSelected();
            if (Input.GetKeyDown(KeyCode.Escape)) BlockDiagramManager.Instance.SetAllUnselected();

            if (Input.GetKeyDown(KeyCode.LeftControl)) shouldMultiSelect = true;
            if (Input.GetKeyUp(KeyCode.LeftControl)) shouldMultiSelect = false;

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) isShiftPressed = true;
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) isShiftPressed = false;

            if (Input.GetKeyDown(KeyCode.LeftAlt)) isAltPressed = true;
            if (Input.GetKeyUp(KeyCode.LeftAlt)) isAltPressed = false;

            // Ctrl + C.
            if (shouldMultiSelect && Input.GetKeyDown(KeyCode.C))
            {

            }

            // Ctrl + V.
            if (shouldMultiSelect && Input.GetKeyDown(KeyCode.V))
            {

            }
        }
    }
}