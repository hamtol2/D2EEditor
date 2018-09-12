using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class PropertyWindowManager : Singleton<PropertyWindowManager>
	{
        [SerializeField] private GeneralNodeWindow generalNodeWindow;
        [SerializeField] private SwitchNodeWindow switchNodeWindow;
        [SerializeField] private VariableNodeWindow variableNodeWindow;
    }
}