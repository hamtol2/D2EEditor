using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using EPaneType = REEL.EAIEditor.EditorManager.EPaneType;

namespace REEL.EAIEditor
{
    public class BlockDiagramManager : Singleton<BlockDiagramManager>
    {
        public NodeBlockArray arrayData = new NodeBlockArray();
        public string filePath = "/Scripts/REEL.EAIEditor/Data/Block.json";

        private NodeBlock testData;
        private int blockId = 0;

        private void Awake()
        {
            filePath = Application.dataPath + filePath;
            //CreateDummyData();
        }

        public void AddBlock(NodeBlock block)
        {
            block.id = blockId++;
            arrayData.Add(block);
        }

        private void CreateDummyData()
        {
            int index = 0;
            while (index < 5)
            {
                testData = new NodeBlock();
                testData.nodeType = (NodeType)index;
                testData.id = index;
                testData.position = new Vector2(index * 30f, index * 40f);

                arrayData.Add(testData);

                ++index;
            }
        }

        private void Start()
        {
            //SaveToFile();
            //LoadFromFile();
        }

        public void LoadFromFile()
        {
            // Load json text and convert to block array.
            NodeBlockArray blockData = LoadJsonData();

            // create blocks on to pane with block array.
            CreateBlocks(blockData);
        }

        private NodeBlockArray LoadJsonData()
        {
            string text = File.ReadAllText(filePath);
            //NodeBlockArray blockData = JsonUtility.FromJson<NodeBlockArray>(text);

            return JsonUtility.FromJson<NodeBlockArray>(text);
        }

        private void CreateBlocks(NodeBlockArray blockData)
        {
            GameObject paneObj = EditorManager.Instance.GetPaneObject(EPaneType.Graph_Pane);
            GraphPane pane = paneObj.GetComponent<GraphPane>();

            for (int ix = 0; ix < blockData.Length; ++ix)
            {
                GraphItem prefab = EditorManager.Instance.GetNodePrefab(blockData[ix].nodeType);
                pane.AddNodeItem(prefab.gameObject, blockData[ix].position, blockData[ix].nodeType);
            }
        }

        public void SaveToFile()
        {
            string jsonString = JsonUtility.ToJson(arrayData);
            File.WriteAllText(filePath, jsonString);
        }
    }
}