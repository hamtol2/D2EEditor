using System;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    [Serializable]
    public class NodeBlock
    {
        public NodeType nodeType = NodeType.START;
        public int id = -1;
        public string title;
        public string name;
        public string value;
        public string variableOperator;
        public Vector2 position;
        public NodeType branchType = NodeType.START;
        public int switchBlockCount = 0;
        public SwitchBlockValueArray switchBlockValues = new SwitchBlockValueArray();
        public int trueNextID;
        public int falseNextID;

        public void SetName(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class SwitchBlockValueArray
    {
        [SerializeField] private string[] blockValues;

        public int Lengh { get { return blockValues == null ? -1 : blockValues.Length; } }

        public string this[int index]
        {
            get { return blockValues[index]; }
            set { blockValues[index] = value; }
        }

        public void Add(string value)
        {
            if (blockValues == null)
            {
                blockValues = new string[1];
                blockValues[0] = value;
                return;
            }

            string[] tempArray = new string[blockValues.Length];
            for (int ix = 0; ix < tempArray.Length; ++ix)
            {
                tempArray[ix] = blockValues[ix];
            }

            blockValues = new string[tempArray.Length + 1];
            for (int ix = 0; ix < tempArray.Length; ++ix)
            {
                blockValues[ix] = tempArray[ix];
            }

            blockValues[blockValues.Length - 1] = value;
        }
    }

    [Serializable]
    public class NodeBlockArray
    {
        [SerializeField]
        private NodeBlock[] nodeData;

        public int Length
        {
            get { return nodeData == null ? -1 : nodeData.Length; }
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