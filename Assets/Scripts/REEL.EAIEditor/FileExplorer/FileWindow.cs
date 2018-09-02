using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    public class FileWindow : MonoBehaviour
    {
        [SerializeField] protected string fileListItemName = "FileListItem";
        [SerializeField] protected FileExplorer fileExplorer;
        [SerializeField] protected RectTransform fileListContent;
        [SerializeField] protected TabManager tabManager;
        [SerializeField] protected string dataPath;

        [SerializeField] protected string selectedProjectFileName = string.Empty;

        [SerializeField] protected List<FileListItem> fileList = new List<FileListItem>();

        protected virtual void Awake()
        {
            dataPath = Application.dataPath + "/Data";
        }

        public virtual void SetSelectedProjectFileName(Text fileNameText)
        {
            selectedProjectFileName = fileNameText.text;
        }

        protected virtual void ResetList()
        {
            if (fileList.Count == 0) return;

            for (int ix = 0; ix < fileList.Count; ++ix)
            {
                ReturnObject(fileListItemName, fileList[ix].gameObject, ObjectPool.Instance.transform);
            }

            fileList = new List<FileListItem>();
        }

        protected virtual void ReturnObject(string objName, GameObject listObj, Transform parent)
        {
            ObjectPool.Instance.PushToPool(objName, listObj, parent);
        }
    }
}