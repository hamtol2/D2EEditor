using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class ExecuteSwitchPoint : ExecutePoint
	{
        public enum SwitchPointType { Case, Default, None }
        [SerializeField] private SwitchPointType pointType = SwitchPointType.Case;
        public SwitchPointType GetSwitchPointType { get { return pointType; } }
	}
}