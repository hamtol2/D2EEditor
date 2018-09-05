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

        public void LoadProject(string fileName)
        {
            projectData = new ProjectData();
            projectData.LoadFromFile(fileName);
        }

        public void SaveProject(string fileName)
        {

        }

        public ProjectData GetProjectData { get { return projectData; } }
    }
}