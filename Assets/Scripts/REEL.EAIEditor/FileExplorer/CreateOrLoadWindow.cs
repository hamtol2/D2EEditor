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
        [SerializeField] private FileExplorer fileExplorerWindow;

        public void OnCloseClicked()
        {
            newAndLoadWindow.SetActive(false);
            fileExplorerWindow.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }

        public void OnOpenClicked()
        {
            if (!tabManager.CanAddTab) return;

            gameObject.SetActive(true);
            newAndLoadWindow.SetActive(true);
        }

        public void OpenSaveWindow()
        {
            gameObject.SetActive(true);
            fileExplorerWindow.gameObject.SetActive(true);
            fileExplorerWindow.OpenSaveWindow();
        }

        public void OpenLoadWindow()
        {
            gameObject.SetActive(true);
            fileExplorerWindow.gameObject.SetActive(true);
            fileExplorerWindow.OpenLoadWindow();
        }

        public void OnLoadClicked()
        {
            newAndLoadWindow.SetActive(false);
            fileExplorerWindow.gameObject.SetActive(true);
        }
	}
}