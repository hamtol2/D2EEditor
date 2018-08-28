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

        [SerializeField] private int blockId = 1;

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
                if (locatedLineList[ix].GetLeftExecutePointInfo.blockID.Equals(graphLine.GetLeftExecutePointInfo.blockID)
                    && locatedLineList[ix].GetRightExecutePointInfo.blockID.Equals(graphLine.GetRightExecutePointInfo.blockID))
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
            for (int ix = 0; ix < lineData.Length; ++ix)
            {
                GraphItem leftItem = GetGraphItem(lineData[ix].left.blockID);
                GraphItem rightItem = GetGraphItem(lineData[ix].right.blockID);

                ExecutePoint leftPoint = null;
                ExecutePoint rightPoint = null;

                if (leftItem != null) leftPoint = leftItem.GetExecutePoint(ExecutePoint.PointPosition.ExecutePoint_Right, lineData[ix].left.executePointID);
                if (rightItem != null) rightPoint = rightItem.GetExecutePoint(ExecutePoint.PointPosition.ExecutePoint_Left);

                if (leftItem != null && rightItem != null && leftPoint != null && rightPoint != null)
                {
                    leftPoint.SetLineData(rightPoint);
                }
            }
        }

        Dictionary<int, int> changedIDTable;

        // Copy/Paste를 통해 블록 복제할 때 사용하는 메소드.
        public void DuplicateBlocks(List<GraphItem> blocks)
        {
            GameObject paneObj = EditorManager.Instance.GetPaneObject(EPaneType.Graph_Pane);
            GraphPane pane = paneObj.GetComponent<GraphPane>();

            // 모든 블록 선택 해제.
            SetAllUnselected();

            // 변경 이전의 ID와 변경된 ID 저장용 딕셔너리 초기화.
            changedIDTable = new Dictionary<int, int>();

            for (int ix = 0; ix < blocks.Count; ++ix)
            {
                GraphItem prefab = EditorManager.Instance.GetNodePrefab(blocks[ix].GetNodeType);
                Vector3 blockPos = blocks[ix].GetComponent<RectTransform>().position;
                blockPos += new Vector3(25f, -25f, 0f);
                pane.AddNodeItem(prefab.gameObject, blockPos, blocks[ix].GetNodeType, blockId++);

                // 추가된 블록 선택.
                SetSelectedGraphItem(locatedItemList[locatedItemList.Count - 1]);

                // 변경전 ID 값 - 변경된 ID 값 기록.
                changedIDTable.Add(blocks[ix].BlockID, blockId - 1);
            }
        }

        // Copy/Paste를 통해 라인 복제할 때 사용하는 메소드.
        public void DuplicateLines(List<GraphLine> lines)
        {
            for (int ix = 0; ix < lines.Count; ++ix)
            {
                int leftID = changedIDTable[lines[ix].GetLeftExecutePointInfo.blockID];
                int rightID = changedIDTable[lines[ix].GetRightExecutePointInfo.blockID];
                GraphItem leftItem = GetGraphItem(leftID);
                GraphItem rightItem = GetGraphItem(rightID);

                ExecutePoint leftPoint = null;
                ExecutePoint rightPoint = null;

                if (leftItem != null) leftPoint = leftItem.GetExecutePoint(ExecutePoint.PointPosition.ExecutePoint_Right, lines[ix].GetLeftExecutePointInfo.executePointID);
                if (rightItem != null) rightPoint = rightItem.GetExecutePoint(ExecutePoint.PointPosition.ExecutePoint_Left);

                if (leftItem != null && rightItem != null && leftPoint != null && rightPoint != null)
                {
                    GraphLine newLine = leftPoint.SetLineData(rightPoint);
                    if (newLine) newLine.SetSelected();
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

        void SaveBlockData()
        {
            NodeBlockArray arrayData = new NodeBlockArray();
            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                NodeBlock block = new NodeBlock()
                {
                    id = locatedItemList[ix].BlockID,
                    nodeType = locatedItemList[ix].GetNodeType,
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
        }

        void SaveLineData()
        {
            LineBlockArray lineBlockArray = new LineBlockArray();
            for (int ix = 0; ix < locatedLineList.Count; ++ix)
            {
                int leftBlockID = locatedLineList[ix].GetLeftExecutePointInfo.blockID;
                int leftExecutePointID = locatedLineList[ix].GetLeftExecutePointInfo.executePointID;
                int rightBlockID = locatedLineList[ix].GetRightExecutePointInfo.blockID;

                LineBlock line = new LineBlock(leftBlockID, leftExecutePointID, rightBlockID);
                lineBlockArray.Add(line);
            }

            string jsonString = JsonUtility.ToJson(lineBlockArray);
            File.WriteAllText(lineFilePath, jsonString);
        }

        public void SaveToFile()
        {
            // Save Block Data.
            SaveBlockData();

            // Save Line Data.
            SaveLineData();
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

            for (int ix = 0; ix < curSelectedLineList.Count; ++ix)
            {
                curSelectedLineList[ix].SetUnselected();
            }

            curSelectedLineList = new List<GraphLine>();
        }

        public int SetBlockUnSelected(GraphItem graphItem)
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

        public void SetLineUnSelected(GraphLine graphLine)
        {
            for (int ix = 0; ix < curSelectedLineList.Count; ++ix)
            {
                if (curSelectedLineList[ix].GetLeftExecutePointInfo.blockID.Equals(graphLine.GetLeftExecutePointInfo.blockID) &&
                    curSelectedLineList[ix].GetRightExecutePointInfo.blockID.Equals(graphLine.GetRightExecutePointInfo.blockID))
                {
                    curSelectedLineList[ix].SetUnselected();
                    curSelectedLineList.RemoveAt(ix);
                }
            }
        }

        private bool CheckIfBlockSelected(GraphItem graphItem)
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

        private bool CheckIfLineSelected(GraphLine graphLine)
        {
            for (int ix = 0; ix < curSelectedLineList.Count; ++ix)
            {
                if (curSelectedLineList[ix].GetLeftExecutePointInfo.blockID.Equals(graphLine.GetLeftExecutePointInfo.blockID)
                    && curSelectedLineList[ix].GetRightExecutePointInfo.blockID.Equals(graphLine.GetRightExecutePointInfo.blockID))
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
            int removeIndex = SetBlockUnSelected(graphItem);

            if (removeIndex == -1)
            {
                curSelectedItemList.Add(graphItem);
                graphItem.SetSelected();
            }
        }

        public void SetBlockSelectedByDrag(GraphItem graphItem)
        {
            if (!CheckIfBlockSelected(graphItem))
            {
                curSelectedItemList.Add(graphItem);
                graphItem.SetSelected();
            }
        }

        public void SetLineSelectionByDrag(GraphLine graphLine)
        {
            if (!CheckIfLineSelected(graphLine))
            {
                curSelectedLineList.Add(graphLine);
                graphLine.SetSelected();
            }
        }

        public List<GraphItem> GetCurrentSelectedBlockList { get { return curSelectedItemList; } }
        public int GetCurrentSelectedBlockCount { get { return curSelectedItemList.Count; } }

        public List<GraphLine> GetCurrentSelectedLineList { get { return curSelectedLineList; } }
        public int GetCurrentSelectedLineCount {  get { return curSelectedLineList.Count; } }

        private void DeleteSelectedBlock()
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

        private void DeleteSelectedLine()
        {
            if (curSelectedLineList == null || curSelectedLineList.Count == 0) return;

            for (int ix = 0; ix < curSelectedLineList.Count; ++ix)
            {
                RemoveLine(curSelectedLineList[ix]);

                Destroy(curSelectedLineList[ix].gameObject);
            }

            curSelectedLineList = new List<GraphLine>();
        }

        public void DeleteSelected()
        {
            DeleteSelectedBlock();
            DeleteSelectedLine();
        }

        // 드래그로 블록 선택하기.
        public void SetBlockSelectionWithDragArea(DragInfo dragInfo)
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
                    SetBlockUnSelected(locatedItemList[ix]);
                    continue;
                }

                SetBlockSelectedByDrag(locatedItemList[ix]);
            }
        }

        // 드래그로 라인 선택하기.
        public void SetLineSelectedWithDragArea(DragInfo dragInfo)
        {
            if (locatedLineList.Count == 0) return;

            for (int ix = 0; ix < locatedLineList.Count; ++ix)
            {
                // 라인 정보 구하기.
                GraphLine.LinePoint linePoint = locatedLineList[ix].GetLinePoint;

                // 드래그 영역의 4개의 점 구하기.
                Vector2 topLeft = dragInfo.topLeft;
                Vector2 topRight = new Vector2(dragInfo.topLeft.x + dragInfo.width, dragInfo.topLeft.y);
                Vector2 lowerLeft = new Vector2(dragInfo.topLeft.x, dragInfo.topLeft.y - dragInfo.height);
                Vector2 lowerRight = new Vector2(dragInfo.topLeft.x + dragInfo.width, dragInfo.topLeft.y - dragInfo.height);

                // 충돌 여부 확인.
                bool top = Util.CheckLineIntersect(linePoint.start, linePoint.end, topLeft, topRight);
                bool left = Util.CheckLineIntersect(linePoint.start, linePoint.end, topLeft, lowerLeft);
                bool right = Util.CheckLineIntersect(linePoint.start, linePoint.end, topRight, lowerRight);
                bool bottom = Util.CheckLineIntersect(linePoint.start, linePoint.end, lowerLeft, lowerRight);

                // 라인이 드래그 영역에 포함되는지 여부 확인.
                bool isIncluded = Util.CheckLineIncluded(linePoint, dragInfo);

                // 충돌.
                if (top || left || right || bottom || isIncluded)
                {
                    SetLineSelectionByDrag(locatedLineList[ix]);
                    continue;
                }
                else
                {
                    SetLineUnSelected(locatedLineList[ix]);
                }
            }
        }
    }
}