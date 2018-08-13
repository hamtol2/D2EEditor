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
        public string filePath = "/Data/Block.d2eproject";

        private NodeBlock testData;
        private int blockId = 0;

        //[SerializeField]
        //private GraphItem currentSelected;
        [SerializeField]
        private List<GraphItem> curSelectedList = new List<GraphItem>();

        private void Awake()
        {
            // Set file path.

            filePath = Application.dataPath + filePath;

            // Test -> Open File Explorer.
            //System.Diagnostics.Process.Start("explorer.exe", "d2eproject");

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
            if (!Directory.Exists(Application.dataPath + "/Data"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Data");
            }

            File.WriteAllText(filePath, jsonString);
        }

        public void SetAllSelected()
        {
            curSelectedList = new List<GraphItem>();

            Transform paneTransform = EditorManager.Instance.GetPaneObject(EPaneType.Graph_Pane).GetComponent<Transform>();
            GraphItem[] items = paneTransform.GetComponentsInChildren<GraphItem>();
            for (int ix = 0; ix < items.Length; ++ix)
            {
                items[ix].SetSelected();
                curSelectedList.Add(items[ix]);
            }
        }

        public void SetAllUnselected()
        {
            for (int ix = 0; ix < curSelectedList.Count; ++ix)
            {
                curSelectedList[ix].SetUnselected();
            }

            curSelectedList = new List<GraphItem>();
        }

        public int SetUnSelected(GraphItem graphItem)
        {
            for (int ix = 0; ix < curSelectedList.Count; ++ix)
            {
                if (curSelectedList[ix].BlockID.Equals(graphItem.BlockID))
                {
                    curSelectedList[ix].SetUnselected();
                    curSelectedList.RemoveAt(ix);
                    return ix;
                }
            }

            return -1;
        }

        public void SetOneSelected(GraphItem graphItem)
        {
            SetAllUnselected();

            curSelectedList.Add(graphItem);
            graphItem.SetSelected();
        }

        public void SetSelectedGraphItem(GraphItem graphItem)
        {
            int removeIndex = SetUnSelected(graphItem);

            if (removeIndex == -1)
            {
                curSelectedList.Add(graphItem);
                graphItem.SetSelected();
            }
        }

        public List<GraphItem> GetCurrentSelectedList
        {
            get { return curSelectedList; }
        }

        public int GetCurrentSelectedCount
        {
            get { return curSelectedList.Count; }
        }

        public void DeleteSected()
        {
            if (curSelectedList == null || curSelectedList.Count == 0) return;

            for (int ix = 0; ix < curSelectedList.Count; ++ix)
            {
                // 배치된 블록 정보 삭제.
                RemoveBlock(curSelectedList[ix].BlockID);

                // 게임 오브젝트 삭제.
                Destroy(curSelectedList[ix].gameObject);
            }

            // 초기화.
            curSelectedList = new List<GraphItem>();
        }
    }
}