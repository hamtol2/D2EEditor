using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    [Serializable]
    public class NodeBlock
    {
        public NodeType nodeType = NodeType.Event;
        public int id = -1;
        public Vector2 position;

        public void UpdatePosition(PointerEventData eventData)
        {
            this.position = eventData.position;
        }
    }

    [Serializable]
    public class NodeBlockArray
    {
        [SerializeField]
        private NodeBlock[] nodeData;

        public int Length
        {
            get { return nodeData.Length; }
        }

        public NodeBlock this[int index]
        {
            get { return nodeData[index]; }
            set { nodeData[index] = value; }
        }

        public void Add(NodeBlock block)
        {
            if (nodeData == null)
            {
                nodeData = new NodeBlock[1];
                nodeData[0] = block;
                return;
            }

            NodeBlock[] tempArray = new NodeBlock[nodeData.Length];
            for (int ix = 0; ix < tempArray.Length; ++ix)
            {
                tempArray[ix] = nodeData[ix];
            }

            nodeData = new NodeBlock[nodeData.Length + 1];
            for (int ix = 0; ix < tempArray.Length; ++ix)
            {
                nodeData[ix] = tempArray[ix];
            }

            nodeData[tempArray.Length] = block;
        }

        public void RemoveAt(int index)
        {
            if (nodeData == null) return;
            if (nodeData.Length == 1)
            {
                nodeData = null;
                return;
            }

            NodeBlock[] tempArray = new NodeBlock[nodeData.Length - 1];
            int idx = 0;
            for (int ix = 0; ix < nodeData.Length; ++ix)
            {
                if (ix == index) continue;
                tempArray[idx] = nodeData[ix];
                ++idx;
            }

            nodeData = new NodeBlock[tempArray.Length];
            for (int ix = 0; ix < nodeData.Length; ++ix)
            {
                nodeData[ix] = tempArray[ix];
            }
        }

        public void Remove(NodeBlock block)
        {
            int index = 0;

            while (!nodeData[index].id.Equals(block.id))
            {
                ++index;
            }

            RemoveAt(index);
        }
    }

    [Serializable]
    public class LineExecutePoint
    {
        public int blockID;

        public LineExecutePoint(int blockID)
        {
            this.blockID = blockID;
        }
    }

    [Serializable]
    public class LineBlock
    {
        public LineExecutePoint left;
        public LineExecutePoint right;

        public LineBlock(int leftBlockID, int rightBlockID)
        {
            left = new LineExecutePoint(leftBlockID);
            right = new LineExecutePoint(rightBlockID);
        }
    }

    [Serializable]
    public class LineBlockArray
    {
        [SerializeField] private LineBlock[] lineData;

        public int Length
        {
            get { return lineData.Length; }
        }

        public LineBlock this[int index]
        {
            get { return lineData[index]; }
            set { lineData[index] = value; }
        }

        public void Add(LineBlock block)
        {
            if (lineData == null)
            {
                lineData = new LineBlock[1];
                lineData[0] = block;
                return;
            }

            LineBlock[] tempArray = new LineBlock[lineData.Length];
            for (int ix = 0; ix < tempArray.Length; ++ix)
            {
                tempArray[ix] = lineData[ix];
            }

            lineData = new LineBlock[lineData.Length + 1];
            for (int ix = 0; ix < tempArray.Length; ++ix)
            {
                lineData[ix] = tempArray[ix];
            }

            lineData[tempArray.Length] = block;
        }
    }
}