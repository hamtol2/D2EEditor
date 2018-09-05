using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class TabData : MonoBehaviour
    {
        [SerializeField] private Project project;

        public void CreateNewProject(string projectName)
        {
            project = new Project();
        }

        public void LoadProject(string fileName)
        {
            project = new Project();
            project.LoadProject(fileName);
        }

        public Project GetProject { get { return project; } }
        public ProjectData GetProjectData { get { return project.GetProjectData; } }
    }
}