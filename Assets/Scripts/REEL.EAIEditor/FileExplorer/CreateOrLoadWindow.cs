using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class CreateOrLoadWindow : MonoBehaviour
	{
        [SerializeField] private TabManager tabManager;
        [SerializeField] private GameObject newAndLoadWindow;
        [SerializeField] private GameObject fileExplorerWindow;

        public void OnCloseClicked()
        {
            newAndLoadWindow.SetActive(true);
            fileExplorerWindow.SetActive(false);

            gameObject.SetActive(false);
        }

        public void OnOpenClicked()
        {
            if (!tabManager.CanAddTab) return;

            gameObject.SetActive(true);
        }

        public void OnLoadClicked()
        {
            //foreach (string file in Directory.GetFiles(Application.dataPath + "/Data", "*.json"))
            //{
            //    string[] test = file.Split(new char[] { '/', '\\' });
            //    Debug.Log(test[test.Length - 1]);
            //}

            newAndLoadWindow.SetActive(false);
            fileExplorerWindow.SetActive(true);
        }
	}
}