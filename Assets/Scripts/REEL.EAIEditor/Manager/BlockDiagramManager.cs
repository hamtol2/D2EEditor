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
        public string itemFilePath = "/Data/Block.json";
        public string lineFilePath = "/Data/Line.json";

        [SerializeField] private int blockId = 0;

        // Block and Line List.
        [SerializeField] private List<GraphItem> locatedItemList = new List<GraphItem>();
        [SerializeField] private List<GraphItem> curSelectedItemList = new List<GraphItem>();

        [SerializeField] private List<GraphLine> locatedLineList = new List<GraphLine>();
        [SerializeField] private List<GraphLine> curSelectedLineList = new List<GraphLine>();

        private void Awake()
        {
            // Set file path.
            itemFilePath = Application.dataPath + itemFilePath;
            lineFilePath = Application.dataPath + lineFilePath;
        }

        public int AddBlock(GraphItem graphItem)
        {
            graphItem.BlockID = graphItem.BlockID == -1 ? blockId++ : graphItem.BlockID;
            locatedItemList.Add(graphItem);

            return graphItem.BlockID;
        }

        public void AddLine(GraphLine graphLine)
        {
            locatedLineList.Add(graphLine);
        }

        public void RemoveLine(GraphLine graphLine)
        {
            if (locatedLineList == null || locatedLineList.Count == 0) return;

            for (int ix = 0; ix < locatedLineList.Count; ++ix)
            {
                if (locatedLineList[ix].LeftBlockID.Equals(graphLine.LeftBlockID) 
                    && locatedLineList[ix].RightBlockID.Equals(graphLine.RightBlockID))
                {
                    locatedLineList.RemoveAt(ix);
                    Destroy(graphLine.gameObject);
                }
            }
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

            if (locatedItemList.Count == 0) blockId = 0;
        }

        private void Start()
        {
            //SaveToFile();
            //LoadFromFile();
        }

        public void SetDragOffset(Vector3 pointerPosition)
        {
            for (int ix = 0; ix < curSelectedItemList.Count; ++ix)
            {
                curSelectedItemList[ix].GetComponent<DragItem>().SetDragOffset(pointerPosition);
            }
        }

        public void BlockDrag(PointerEventData eventData, GraphItem graphItem = null)
        {
            // 드래드시 동작 현재 마우스 포인터가 위치한 블록 선택하기.
            if (graphItem != null && curSelectedItemList.Count == 1)
            {
                if (curSelectedItemList[0].BlockID != graphItem.BlockID)
                {
                    curSelectedItemList[0].SetUnselected();

                    graphItem.GetComponent<DragItem>().SetDragOffset(eventData.position);
                    graphItem.SetSelected();

                    curSelectedItemList[0] = graphItem;
                }   
            }

            for (int ix = 0; ix < curSelectedItemList.Count; ++ix)
            {
                curSelectedItemList[ix].GetComponent<DragItem>().ChangePosition(eventData);
            }
        }

        public void LoadFromFile()
        {
            // Load json text and convert to block array.
            // create blocks on to pane with block array.
            CreateBlocks(LoadItemDataFromJson());
            CreateLines(LoadLineDataFromJson());
        }

        private NodeBlockArray LoadItemDataFromJson()
        {
            return JsonUtility.FromJson<NodeBlockArray>(File.ReadAllText(itemFilePath));
        }

        private LineBlockArray LoadLineDataFromJson()
        {
            return JsonUtility.FromJson<LineBlockArray>(File.ReadAllText(lineFilePath));
        }

        private void CreateBlocks(NodeBlockArray blockData)
        {
            GameObject paneObj = EditorManager.Instance.GetPaneObject(EPaneType.Graph_Pane);
            GraphPane pane = paneObj.GetComponent<GraphPane>();

            int maxID = blockId;

            for (int ix = 0; ix < blockData.Length; ++ix)
            {
                GraphItem prefab = EditorManager.Instance.GetNodePrefab(blockData[ix].nodeType);
                pane.AddNodeItem(prefab.gameObject, blockData[ix].position, blockData[ix].nodeType, blockData[ix].id);
                
                if (blockData[ix].id >= maxID) maxID = blockData[ix].id + 1;
            }

            blockId = maxID;
        }

        private void CreateLines(LineBlockArray lineData)
        {
            for (int ix = 0; ix < lineData.Length; ++ ix)
            {
                GraphItem leftItem = GetGraphItem(lineData[ix].left.blockID);
                GraphItem rightItem = GetGraphItem(lineData[ix].right.blockID);

                ExecutePoint leftPoint = null;
                ExecutePoint rightPoint = null;

                if (leftItem != null) leftPoint = leftItem.GetExecutePoint(ExecutePoint.PointPosition.ExecutePoint_Right);
                if (rightItem != null) rightPoint = rightItem.GetExecutePoint(ExecutePoint.PointPosition.ExecutePoint_Left);

                if (leftItem != null && rightItem != null && leftPoint != null && rightPoint != null)
                {
                    leftPoint.SetLineData(rightPoint);
                }
            }
        }

        private GraphItem GetGraphItem(int blockID)
        {
            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                if (locatedItemList[ix].BlockID.Equals(blockID))
                    return locatedItemList[ix];
            }

            return null;
        }

        public void SaveToFile()
        {
            // Save Block Data.
            // ======================================================= //
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

            File.WriteAllText(itemFilePath, jsonString);
            // ======================================================= //

            // Save Line Data.
            // ======================================================= //
            LineBlockArray lineBlockArray = new LineBlockArray();
            for (int ix = 0; ix < locatedLineList.Count; ++ix)
            {
                LineBlock line = new LineBlock(locatedLineList[ix].LeftBlockID, locatedLineList[ix].RightBlockID);

                lineBlockArray.Add(line);
            }

            jsonString = JsonUtility.ToJson(lineBlockArray);
            File.WriteAllText(lineFilePath, jsonString);
            // ======================================================= //
        }

        public void SetAllSelected()
        {
            curSelectedItemList = new List<GraphItem>();

            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                locatedItemList[ix].SetSelected();
                curSelectedItemList.Add(locatedItemList[ix]);
            }
        }

        public void SetAllUnselected()
        {
            for (int ix = 0; ix < curSelectedItemList.Count; ++ix)
            {
                curSelectedItemList[ix].SetUnselected();
            }

            curSelectedItemList = new List<GraphItem>();
        }

        public int SetUnSelected(GraphItem graphItem)
        {
            for (int ix = 0; ix < curSelectedItemList.Count; ++ix)
            {
                if (curSelectedItemList[ix].BlockID.Equals(graphItem.BlockID))
                {
                    curSelectedItemList[ix].SetUnselected();
                    curSelectedItemList.RemoveAt(ix);
                    return ix;
                }
            }

            return -1;
        }

        private bool CheckIfSelected(GraphItem graphItem)
        {
            for (int ix = 0; ix < curSelectedItemList.Count; ++ix)
            {
                if (curSelectedItemList[ix].BlockID.Equals(graphItem.BlockID))
                {
                    return true;
                }
            }

            return false;
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
                curSelectedItemList.Add(graphItem);
                graphItem.SetSelected();
            }
        }

        public void SetSelectedByDrag(GraphItem graphItem)
        {
            if (!CheckIfSelected(graphItem))
            {
                curSelectedItemList.Add(graphItem);
                graphItem.SetSelected();
            }
        }

        public List<GraphItem> GetCurrentSelectedList
        {
            get { return curSelectedItemList; }
        }

        public int GetCurrentSelectedCount
        {
            get { return curSelectedItemList.Count; }
        }

        public void DeleteSelected()
        {
            if (curSelectedItemList == null || curSelectedItemList.Count == 0) return;

            for (int ix = 0; ix < curSelectedItemList.Count; ++ix)
            {
                // 배치된 블록 정보 삭제.
                RemoveBlock(curSelectedItemList[ix]);

                // 게임 오브젝트 삭제.
                Destroy(curSelectedItemList[ix].gameObject);
            }

            // 초기화.
            curSelectedItemList = new List<GraphItem>();
        }

        public void SetSelectionWithDragArea(DragInfo dragInfo)
        {
            if (locatedItemList.Count == 0) return;

            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                RectTransform itemRect = locatedItemList[ix].GetComponent<RectTransform>();

                float itemHalfWidth = itemRect.sizeDelta.x * 0.5f;
                float itemHalfHeight = itemRect.sizeDelta.y * 0.5f;

                if (dragInfo.topLeft.x > itemRect.position.x + itemHalfWidth ||
                    dragInfo.topLeft.x + dragInfo.width < itemRect.position.x - itemHalfWidth ||
                    dragInfo.topLeft.y < itemRect.position.y - itemHalfHeight ||
                    dragInfo.topLeft.y - dragInfo.height > itemRect.position.y + itemHalfHeight)
                {
                    SetUnSelected(locatedItemList[ix]);
                    continue;
                }

                SetSelectedByDrag(locatedItemList[ix]);
            }
        }
    }
}