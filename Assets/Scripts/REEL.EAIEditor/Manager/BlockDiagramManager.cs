using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using EPaneType = REEL.EAIEditor.EditorManager.EPaneType;

namespace REEL.EAIEditor
{
    public class BlockDiagramManager : Singleton<BlockDiagramManager>
    {
        public string filePath = "/Data/Block.d2eproject";

        private NodeBlock testData;
        private int blockId = 0;

        [SerializeField]
        private List<GraphItem> locatedItemList = new List<GraphItem>();

        [SerializeField]
        private List<GraphItem> curSelectedList = new List<GraphItem>();

        private void Awake()
        {
            // Set file path.
            filePath = Application.dataPath + filePath;
        }

        public int AddBlock(GraphItem graphItem)
        {
            graphItem.BlockID = graphItem.BlockID == -1 ? blockId++ : graphItem.BlockID;
            locatedItemList.Add(graphItem);

            return graphItem.BlockID;
        }

        public void RemoveBlock(GraphItem graphItem)
        {
            if (locatedItemList == null || locatedItemList.Count == 0) return;

            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                if (locatedItemList[ix].BlockID.Equals(graphItem.BlockID))
                {
                    locatedItemList.RemoveAt(ix);
                    Destroy(graphItem.gameObject);
                }
            }
        }

        private void Start()
        {
            //SaveToFile();
            //LoadFromFile();
        }

        public void SetDragOffset(Vector3 pointerPosition)
        {
            for (int ix = 0; ix < curSelectedList.Count; ++ix)
            {
                curSelectedList[ix].GetComponent<DragItem>().SetDragOffset(pointerPosition);
            }
        }

        //public void BlockDrag(PointerEventData eventData)
        public void BlockDrag(PointerEventData eventData, GraphItem graphItem = null)
        {
            // 드래드시 동작 현재 마우스 포인터가 위치한 블록 선택하기.
            if (graphItem != null && curSelectedList.Count == 1)
            {
                if (curSelectedList[0].BlockID != graphItem.BlockID)
                {
                    curSelectedList[0].SetUnselected();

                    graphItem.GetComponent<DragItem>().SetDragOffset(eventData.position);
                    graphItem.SetSelected();

                    curSelectedList[0] = graphItem;
                }   
            }

            for (int ix = 0; ix < curSelectedList.Count; ++ix)
            {
                curSelectedList[ix].GetComponent<DragItem>().ChangePosition(eventData);
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
                pane.AddNodeItem(prefab.gameObject, blockData[ix].position, blockData[ix].nodeType, blockData[ix].id);
            }
        }

        public void SaveToFile()
        {
            NodeBlockArray arrayData = new NodeBlockArray();
            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                NodeBlock block = new NodeBlock()
                {
                    id = locatedItemList[ix].BlockID,
                    nodeType = locatedItemList[ix].nodeType,
                    position = locatedItemList[ix].GetComponent<RectTransform>().position
                };

                arrayData.Add(block);
            }

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

            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                locatedItemList[ix].SetSelected();
                curSelectedList.Add(locatedItemList[ix]);
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
            SetSelectedGraphItem(graphItem);
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
                RemoveBlock(curSelectedList[ix]);

                // 게임 오브젝트 삭제.
                Destroy(curSelectedList[ix].gameObject);
            }

            // 초기화.
            curSelectedList = new List<GraphItem>();
        }
    }
}