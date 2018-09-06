using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace REEL.EAIEditor
{
    [System.Serializable]
    public class Project
    {
        [SerializeField] private ProjectData projectData;

        private string dataPath = string.Empty;

        public Project()
        {
            projectData = new ProjectData();
        }

        public Project(string projectName)
        {
            projectData = new ProjectData(projectName);
        }

        public void LoadProject(string fileName)
        {
            projectData = new ProjectData();
            projectData.LoadFromFile(fileName);
        }

        public void SaveProject(ProjectFormat format)
        {
            projectData.SaveToFile(format);
        }

        public ProjectData GetProjectData { get { return projectData; } }
    }
}