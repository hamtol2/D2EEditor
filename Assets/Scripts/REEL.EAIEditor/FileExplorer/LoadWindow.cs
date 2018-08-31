using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    public class LoadWindow : MonoBehaviour
    {
        [SerializeField] private string fileListItemName = "FileListItem";
        [SerializeField] private RectTransform fileListContent;
        [SerializeField] private string dataPath;

        [SerializeField] private string selectedProjectFileName = string.Empty;

        private void Awake()
        {
            dataPath = Application.dataPath + "/Data";
        }

        public void SetSelectedProjectFileName(Text fileNameText)
        {
            selectedProjectFileName = fileNameText.text;
        }

        private void OnEnable()
        {
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

                EventTrigger trigger = list.GetComponent<EventTrigger>();                
            }
        }
    }
}