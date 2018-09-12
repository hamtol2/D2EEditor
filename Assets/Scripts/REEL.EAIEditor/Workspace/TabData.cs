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
            project = new Project(projectName);
        }

        public void LoadProject(string fileName)
        {
            project = new Project();
            project.LoadProject(fileName);
        }

        public void SaveProject(ProjectFormat format)
        {
            project.SaveProject(format);
        }

        public void SaveState()
        {
            if (!IsValid)
            {
                //Debug.Log("SaveState null");
                return;
            }

            ProjectFormat format = WorkspaceManager.Instance.GetSaveFormat(GetProjectData.GetProjectFormat.projectName);

            //GetProjectData.UpdateProjectState(WorkspaceManager.Instance.GetSaveFormat(GetProjectData.GetProjectFormat.projectName));
            GetProjectData.UpdateProjectState(format);
        }

        public void ChangeState(bool isSelected)
        {
            if (isSelected && IsValid)
            {
                WorkspaceManager.Instance.LoadFromProjectFormat(GetProjectData.GetProjectFormat);
            }
            else if (!isSelected)
            {
                //SaveState();
                WorkspaceManager.Instance.ReleaseAllLogic();
            }
        }

        public bool IsValid
        {
            get { return GetProjectData != null && GetProjectData.GetProjectFormat != null; }
        }

        public Project GetProject { get { return project; } }
        public ProjectData GetProjectData { get { return project.GetProjectData; } }
    }
}