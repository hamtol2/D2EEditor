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
        public NodeBlock[] nodeData;

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
}