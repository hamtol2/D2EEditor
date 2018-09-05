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

        private ProjectFormat LoadProjectDataFromJson(string projectName = "")
        {
            fileName = (string.IsNullOrEmpty(projectName) ? "Test" : projectName);
            string projectFilePath = SetProjectPath(fileName);
            return JsonUtility.FromJson<ProjectFormat>(File.ReadAllText(projectFilePath));
        }

        public ProjectFormat GetProjectFormat { get { return project; } }
    }
}