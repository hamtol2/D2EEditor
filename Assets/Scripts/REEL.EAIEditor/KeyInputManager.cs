using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class KeyInputManager : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                BlockDiagramManager.Instance.DeleteSected();
            }
        }
    }
}