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
        public string filePath = "/Scripts/REEL.EAIEditor/Data/Block.d2eproject";

        private NodeBlock testData;
        private int blockId = 0;

        //[SerializeField]
        private GraphItem currentSelected;

        private void Awake()
        {
            // Set file path.
            filePath = Application.dataPath + filePath;

            //CreateDummyData();            // for test.
        }

        // Add Block to diagram manager.
        public int AddBlock(NodeBlock block)
        {
            block.id = blockId++;
            arrayData.Add(block);

            // return id.
            return block.id;
        }

        public void RemoveBlock(int blockID)
        {
            for (int ix = 0; ix < arrayData.Length; ++ix)
            {
                if (arrayData[ix].id.Equals(blockID))
                {
                    arrayData.RemoveAt(ix);
                    break;
                }
            }
        }

        private void Start()
        {
            //SaveToFile();
            //LoadFromFile();
        }

        // Create Dummy Data for test.
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

        public void LoadFromFile()
        {
            // Load json text and convert to block array.
            // create blocks on to pane with block array.
            CreateBlocks(LoadJsonData());
        }

        private NodeBlockArray LoadJsonData()
        {
            return JsonUtility.FromJson<NodeBlockArray>(File.ReadAllText(filePath));
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

        public void SetSelectedGraphItem(GraphItem graphItem)
        {
            if (currentSelected != null && currentSelected != graphItem)
                currentSelected.SetUnselected();

            currentSelected = graphItem;
            if (currentSelected) currentSelected.SetSelected();
        }

        public GraphItem GetCurrentSelectedNode
        {
            get { return currentSelected; }
        }

        public void DeleteSected()
        {
            if (!currentSelected) return;

            // 배치된 블록 정보 삭제.
            RemoveBlock(currentSelected.BlockID);

            // 게임 오브젝트 삭제.
            Destroy(currentSelected.gameObject);

            // null로 초기화.
            currentSelected = null;
        }
    }
}