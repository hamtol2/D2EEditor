using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class ClipBoardManager : Singleton<ClipBoardManager>
    {
        // 클립보드에 저장할 내용 정의.
        [System.Serializable]
        public class ClipBoardContent
        {
            public enum ClipboardType
            {
                Copy, Paste, Undo, Redo, Revove, Add
            }

            public ClipboardType clipboardType;
            public List<GraphItem> blocks = new List<GraphItem>();
            public List<GraphLine> lines = new List<GraphLine>();

            public ClipBoardContent(ClipboardType clipboardType, List<GraphItem> blocks = null, List<GraphLine> lines = null)
            {
                this.clipboardType = clipboardType;
                if (blocks != null || blocks.Count > 0)
                {
                    for (int ix = 0; ix < blocks.Count; ++ix)
                    {
                        this.blocks.Add(blocks[ix]);
                    }
                }

                if (lines != null || lines.Count > 0)
                {
                    for (int ix = 0; ix < lines.Count; ++ix)
                    {
                        this.lines.Add(lines[ix]);
                    }
                }
            }
        }

        public List<ClipBoardContent> clipBoardContents = new List<ClipBoardContent>();
        //public Stack<ClipBoardContent> clipBoardContents = new Stack<ClipBoardContent>();

        public void PushContent(ClipBoardContent newContent)
        {
            clipBoardContents.Add(newContent);
            //clipBoardContents.Push(newContent);
        }

        public ClipBoardContent PopContent()
        {
            ClipBoardContent content = clipBoardContents[clipBoardContents.Count - 1];
            clipBoardContents.RemoveAt(clipBoardContents.Count - 1);

            return content;
            //return clipBoardContents.Pop();
        }
    }
}