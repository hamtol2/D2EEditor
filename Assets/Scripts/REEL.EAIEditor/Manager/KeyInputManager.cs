using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    using ClipBoardContent = ClipBoardManager.ClipBoardContent;
    using ClipboardType = ClipBoardManager.ClipBoardContent.ClipboardType;

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
            if (Input.GetKeyDown(KeyCode.C))
            {
#if UNITY_EDITOR
                if (isShiftPressed)
#else
                if (shouldMultiSelect)
#endif
                {
                    List<GraphItem> selectedBlock = BlockDiagramManager.Instance.GetCurrentSelectedBlockList;
                    List<GraphLine> selectedLine = BlockDiagramManager.Instance.GetCurrentSelectedLineList;
                    
                    ClipBoardContent newContent = new ClipBoardContent(ClipboardType.Copy, selectedBlock, selectedLine);
                    ClipBoardManager.Instance.PushContent(newContent);
                }
            }

            // Ctrl + V.
            if (Input.GetKeyDown(KeyCode.V))
            {
#if UNITY_EDITOR
                if (isShiftPressed)
#else
                if (shouldMultiSelect)
#endif
                {
                    ClipBoardContent content = ClipBoardManager.Instance.PopContent();
                    BlockDiagramManager.Instance.DuplicateBlocks(content.blocks);
                    BlockDiagramManager.Instance.DuplicateLines(content.lines);
                }
            }

            // Ctrl + Z.
            if (Input.GetKeyDown(KeyCode.Z))
            {
#if UNITY_EDITOR
                if (isShiftPressed)
#else
                if (shouldMultiSelect)
#endif
                {

                }
            }
        }
    }
}