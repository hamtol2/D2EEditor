using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class FileExplorer : MonoBehaviour
	{
        [SerializeField] private TabManager tabManager;

		public void OnClickedClose()
        {
            gameObject.SetActive(false);
        }

        public void OnClicedOpen()
        {
            if (!tabManager.CanAddTab) return;

            gameObject.SetActive(true);
        }
	}
}