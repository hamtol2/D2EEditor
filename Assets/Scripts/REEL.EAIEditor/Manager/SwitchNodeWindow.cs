using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class SwitchNodeWindow : MonoBehaviour, IShowProperty
	{
		public void ShowProperty(GraphItem node)
        {
            gameObject.SetActive(true);
        }
    }
}