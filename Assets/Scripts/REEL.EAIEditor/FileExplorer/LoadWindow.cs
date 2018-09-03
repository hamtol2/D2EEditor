using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    public class LoadWindow : FileWindow
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

        public void OnLoadClosed()
        {
            inputField.text = string.Empty;
            ResetList();
            fileExplorer.OnCancelClicked();
        }

        public void OnLoadClicked()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;

            tabManager.AddTab(selectedProjectFileName);
            BlockDiagramManager.Instance.LoadFromFile(selectedProjectFileName);
        }
    }
}