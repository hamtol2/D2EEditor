using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace REEL.EAIEditor
{
    using EPaneType = REEL.EAIEditor.EditorManager.EPaneType;

    [System.Serializable]
	public class ProjectData
	{
        [SerializeField] private string fileName;                       // project file name.
        [SerializeField] private string fileExtension = "json";         // development version format.
        [SerializeField] private int blockId = 1;

        // Block and Line List.
        [SerializeField] private List<GraphItem> locatedItemList = new List<GraphItem>();
        [SerializeField] private List<GraphItem> curSelectedItemList = new List<GraphItem>();

        [SerializeField] private List<GraphLine> locatedLineList = new List<GraphLine>();
        [SerializeField] private List<GraphLine> curSelectedLineList = new List<GraphLine>();

        private string SetProjectPath(string fileName)
        {
            // Set project data path
            StringBuilder sb = new StringBuilder();
            sb.Append("Data/");
            sb.Append(fileName);
            sb.Append(".");
            sb.Append(fileExtension);

            return Path.Combine(Application.dataPath, sb.ToString());
        }

        //public void LoadFromFile()
        public void LoadFromFile(string fileName)
        {
            // Load json text and convert to project format.
            //ProjectFormat project = LoadProjectDataFromJson("Test1");
            ProjectFormat project = LoadProjectDataFromJson(fileName);

            // create blocks with project format.
            CreateBlocks(project.blockArray);

            // create lines with project format.
            CreateLines(project.lineArray);
        }

        private ProjectFormat LoadProjectDataFromJson(string projectName = "")
        {
            fileName = (string.IsNullOrEmpty(projectName) ? "Project" : projectName);
            string projectFilePath = SetProjectPath(fileName);
            return JsonUtility.FromJson<ProjectFormat>(File.ReadAllText(projectFilePath));
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

        private GraphItem GetGraphItem(int blockID)
        {
            for (int ix = 0; ix < locatedItemList.Count; ++ix)
            {
                if (locatedItemList[ix].BlockID.Equals(blockID))
                    return locatedItemList[ix];
            }

            return null;
        }
    }
}