using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
	public class SaveWindow : FileWindow
	{
        public void LoadFileList()
        {
            ResetList();

            string[] files = Directory.GetFiles(dataPath, "*.json");

            for (int ix = 0; ix < files.Length; ++ix)
            {
                //Debug.Log(files[ix]);
                GameObject item = ObjectPool.Instance.PopFromPool(fileListItemName, fileListContent);
                item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                item.SetActive(true);

                string[] info = files[ix].Split(new char[] { '/', '\\' });
                string fileName = info[info.Length - 1];
                fileName = fileName.Split(new char[] { '.' })[0];

                FileListItem list = item.GetComponent<FileListItem>();
                list.SetFileName(fileName);
                list.SetFileWindow(this);
                list.SetInputField(inputField);

                fileList.Add(list);

                EventTrigger trigger = list.GetComponent<EventTrigger>();
            }
        }

        public void OnSaveClosed()
        {
            inputField.text = string.Empty;
            ResetList();
            fileExplorer.OnSaveClicked();
        }

        //public void OnSaveClicked()
        //{
        //    if (string.IsNullOrEmpty(inputField.text)) return;

        //    //tabManager.AddTab(selectedProjectFileName);
        //    BlockDiagramManager.Instance.SaveToFile(string.IsNullOrEmpty(selectedProjectFileName) ? "Test" : selectedProjectFileName);
        //}
    }
}