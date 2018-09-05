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
        [SerializeField] private SaveWindow saveWindow = null;
        [SerializeField] private LoadWindow loadWindow = null;

        [SerializeField] private RectTransform fileListContent;
        [SerializeField] private InputField fileNameInput = null;
        [SerializeField] private TabManager tabManager = null;
        [SerializeField] private CreateOrLoadWindow window = null;

        [SerializeField] private string dataPath;

        private void Awake()
        {
            dataPath = Application.dataPath + "/Data";
        }

        public void OpenSaveWindow()
        {
            saveWindow.gameObject.SetActive(true);
            saveWindow.LoadFileList();
            loadWindow.gameObject.SetActive(false);
        }

        public void OpenLoadWindow()
        {
            loadWindow.gameObject.SetActive(true);
            loadWindow.LoadFileList();
            saveWindow.gameObject.SetActive(false);
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