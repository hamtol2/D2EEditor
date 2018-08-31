using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    public class FileListItem : MonoBehaviour
    {
        [SerializeField] private Text fileNameText;
        [SerializeField] private Text typeNameText;

        public void SetFileName(string fileName)
        {
            fileNameText.text = fileName;
        }

        public void SetTypeName(string typeName)
        {
            typeNameText.text = typeName;
        }
    }
}