using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class TabData : MonoBehaviour
    {
        [SerializeField] private Project project;

        public void LoadProject(string fileName)
        {
            project = new Project();
            project.LoadProject(fileName);
        }

        public Project GetProject { get { return project; } }
    }
}