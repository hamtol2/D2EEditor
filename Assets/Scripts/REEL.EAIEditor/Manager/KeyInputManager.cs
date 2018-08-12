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
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                BlockDiagramManager.Instance.DeleteSected();
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