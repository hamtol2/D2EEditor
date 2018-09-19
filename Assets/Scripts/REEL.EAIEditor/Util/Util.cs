using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public static class Util
    {
        public static bool CompareTwoStrings(string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.CurrentCultureIgnoreCase);
        }

        public static float GetAngleBetween(Vector2 start, Vector2 end)
        {
            Vector2 target = end - start;
            return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        }

        public static float GetDistanceBetween(Vector2 start, Vector2 end)
        {
            Vector2 target = end - start;
            return target.magnitude;
        }

        public static T ReadFromJson<T>(TextAsset jsonText)
        {
            return SimpleJson.SimpleJson.DeserializeObject<T>(jsonText.text);
        }

        public static string RemoveAllWhiteSpace(string targetString)
        {
            return targetString.Replace(" ", string.Empty);
        }

        // 두 직선이 서로 교차하는지 확인하는 메소드.
        public static bool CheckLineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float ab = CCW(a, b, c) * CCW(a, b, d);
            float cd = CCW(c, d, a) * CCW(c, d, b);

            return ab <= 0f && cd <= 0f;
        }

        // 직선이 드래그 영역에 포함되는지 확인.
        public static bool CheckLineIncluded(GraphLine.LinePoint linePoint, DragInfo dragInfo)
        {
            if (linePoint.start.x > dragInfo.topLeft.x && linePoint.start.x < dragInfo.topLeft.x + dragInfo.width &&
                linePoint.end.x > dragInfo.topLeft.x && linePoint.end.x < dragInfo.topLeft.x + dragInfo.width &&
                linePoint.start.y < dragInfo.topLeft.y && linePoint.start.y > dragInfo.topLeft.y - dragInfo.height &&
                linePoint.end.y < dragInfo.topLeft.y && linePoint.end.y > dragInfo.topLeft.y - dragInfo.height)
            {
                return true;
            }

            return false;
        }

        private static float CCW(Vector2 a, Vector2 b)
        {
            return a.Cross(b);
        }

        private static float CCW(Vector2 p, Vector2 a, Vector2 b)
        {
            return CCW(a - p, b - p);
        }

        // Vector2 확장 메소드 (외적 계산).
        public static float Cross(this Vector2 myVector, Vector2 otherVector)
        {
            return myVector.x * otherVector.y - myVector.y * otherVector.x;
        }

        // UI RayCaster.
        public static bool GetRaycastResult(BaseRaycaster raycaster, EventSystem eventSystem, Vector2 mousePosition, out RaycastResult result)
        {
            result = new RaycastResult();
            PointerEventData data = new PointerEventData(eventSystem);
            data.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(data, results);

            if (results.Count > 0)
            {
                result = results[0];
                return true;
            }

            else return false;
        }

        public static void XMLSerialize<T>(T node, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, node);
            writer.Close();
        }

        public static T XMLDeserialize<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(path);
            T xmlObject = (T)serializer.Deserialize(reader.BaseStream);
            reader.Close();
            return xmlObject;
        }

        public static ProjectFormat GetSaveFormat(List<GraphItem> locatedItemList, List<GraphLine> locatedLineList, string projectName = "")
        {
            ProjectFormat project = new ProjectFormat();
            project.projectName = projectName;

            project.blockArray = new NodeBlockArray();
            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                NodeBlock block = new NodeBlock();
                block.nodeType = locatedItemList[ix].GetNodeType;
                block.id = locatedItemList[ix].BlockID;
                block.title = locatedItemList[ix].GetBlockTitle;
                block.value = locatedItemList[ix].GetItemData() as string;
                block.position = locatedItemList[ix].GetComponent<RectTransform>().position;

                NodeType nodeType = locatedItemList[ix].GetNodeType;

                if (nodeType == NodeType.SWITCH)
                {
                    SwitchBranchItem switchNode = locatedItemList[ix] as SwitchBranchItem;
                    block.switchBlockCount = switchNode.GetBlockCount;
                    block.switchBranchType = switchNode.GetSwitchType;
                    block.name = switchNode.GetSwitchName;
                    for (int jx = 0; jx < switchNode.GetBlockCount; ++jx)
                    {
                        ExecuteCasePoint casePoint = switchNode.executePoints[jx + 1] as ExecuteCasePoint;
                        block.switchBlockValues.Add(casePoint.CaseValue);
                    }
                }

                else if (nodeType == NodeType.VARIABLE)
                {
                    VariableItem variableNode = locatedItemList[ix] as VariableItem;
                    block.variableOperator = variableNode.GetOperatorType.ToString();
                    block.name = variableNode.GetBlockName;
                }

                else if (nodeType == NodeType.IF)
                {
                    IFBranchItem ifNode = locatedItemList[ix] as IFBranchItem;
                    BranchCondition data = ifNode.GetIFBranchData();
                    block.name = data.nameField;
                    block.value = data.rParameter;
                    block.ifBranchType = data.lParamType;
                    block.variableOperator = ifNode.GetStringFromOpType(data.opParameter);
                }

                project.BlockAdd(block);
            }

            project.lineArray = new LineBlockArray();
            for (int ix = 0; ix < locatedLineList.Count; ++ix)
            {
                int leftBlockID = locatedLineList[ix].GetLeftExecutePointInfo.blockID;
                int leftExecutePointID = locatedLineList[ix].GetLeftExecutePointInfo.executePointID;
                int rightBlockID = locatedLineList[ix].GetRightExecutePointInfo.blockID;

                LineBlock line = new LineBlock(leftBlockID, leftExecutePointID, rightBlockID);
                project.LineAdd(line);
            }

            return project;
        }

        public static void CompileToXML(ProjectFormat projectFormat, List<GraphItem> locatedItemList, IComparer<GraphItem> comparer)
        {
            locatedItemList.Sort(comparer);

            XMLProject project = new XMLProject();

            foreach (GraphItem node in locatedItemList)
            {
                if (node.GetNodeType == NodeType.SWITCH)
                {
                    SwitchBranchItem switchNode = node as SwitchBranchItem;
                    project.AddNode(switchNode.GetXMLSwitchData());
                }
                else if (node.GetNodeType == NodeType.VARIABLE)
                {
                    VariableItem variableNode = node as VariableItem;
                    project.AddNode(variableNode.GetXMLVariableData());
                }
                else if (node.GetNodeType == NodeType.IF)
                {
                    IFBranchItem ifNode = node as IFBranchItem;
                    project.AddNode(ifNode.GetXMLIFData());
                }
                else
                {
                    project.AddNode(node.GetXMLNormalData());
                }
            }

            XMLSerialize<XMLProject>(project, Application.dataPath + "/Data/TestProject.xml");
        }
    }
}