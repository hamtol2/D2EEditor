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

        [SerializeField] private ProjectFormat project;

        public ProjectData()
        {

        }

        public ProjectData(string projectName)
        {
            fileName = projectName;
        }

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
            project = LoadProjectDataFromJson(fileName);
        }

        public void UpdateProjectState(ProjectFormat format)
        {
            Debug.Log(format.projectName + ", " + format.blockArray.Length);
            //project = format;
        }

        public void SaveToFile(ProjectFormat format)
        {
            if (!Directory.Exists(Application.dataPath + "/Data"))
                Directory.CreateDirectory(Application.dataPath + "/Data");

            string projectPath = SetProjectPath(format.projectName);
            string jsonString = JsonUtility.ToJson(format);
            File.WriteAllText(projectPath, jsonString);
        }

        private ProjectFormat LoadProjectDataFromJson(string projectName = "")
        {
            fileName = projectName;
            string projectFilePath = SetProjectPath(fileName);
            return JsonUtility.FromJson<ProjectFormat>(File.ReadAllText(projectFilePath));
        }

        public ProjectFormat GetProjectFormat
        {
            get
            {
                //if (project == null) { project = new ProjectFormat(); }
                return project;
            }
        }
    }
}