using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class FileExplorer : MonoBehaviour
	{
        [SerializeField] private string fileListItemName = "FileListItem";
        [SerializeField] private GameObject saveWindow;
        [SerializeField] private LoadWindow loadWindow;

        [SerializeField] private RectTransform fileListContent;
        [SerializeField] private InputField fileNameInput;
        [SerializeField] private TabManager tabManager;
        [SerializeField] private CreateOrLoadWindow window;

        [SerializeField] private string dataPath;

        private void Awake()
        {
            dataPath = Application.dataPath + "/Data";
        }

        public void OpenSaveWindow()
        {
            saveWindow.SetActive(true);
            loadWindow.gameObject.SetActive(false);
        }

        public void OpenLoadWindow()
        {
            loadWindow.gameObject.SetActive(true);
            loadWindow.LoadFileList();
            saveWindow.SetActive(false);
        }

        public void OnSaveClicked()
        {   
            tabManager.SaveProject(fileNameInput.text);
        }

        public void OnCancelClicked()
        {
            window.OnCloseClicked();
        }

        public CreateOrLoadWindow GetCreateOrLoadWindow { get { return window; } }

        //public void OnEnable()
        //{
        //    string[] files = Directory.GetFiles(dataPath, "*.json");
            
        //    for (int ix = 0; ix < files.Length; ++ix)
        //    {
        //        Debug.Log(files[ix]);
        //        GameObject item = ObjectPool.Instance.PopFromPool(fileListItemName, fileListContent);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        //        //item.GetComponent<RectTransform>().localScale = Vector3.one;
        //        item.SetActive(true);
        //        FileListItem list = item.GetComponent<FileListItem>();
        //        string[] info = files[ix].Split(new char[] { '/', '\\' });
        //        string fileName = info[info.Length - 1];
        //        fileName = fileName.Split(new char[] { '.' })[0];
        //        list.SetFileName(fileName);
        //    }
        //}
    }
}