using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class ClipBoardManager : Singleton<ClipBoardManager>
    {
        // 클립보드에 저장할 내용 정의.
        public class ClipBoardContent
        {
            public enum ClipboardType
            {
                Copy, Paste, Undo, Redo
            }

            public ClipboardType clipboardType;
            public object content;

            public ClipBoardContent(ClipboardType clipboardType, object content)
            {
                this.clipboardType = clipboardType;
                this.content = content;
            }
        }

        private Stack<ClipBoardContent> clipBoardContents = new Stack<ClipBoardContent>();

        public void PushContent(ClipBoardContent newContent)
        {
            clipBoardContents.Push(newContent);
        }

        public ClipBoardContent PopContent()
        {
            return clipBoardContents.Pop();
        }
    }
}