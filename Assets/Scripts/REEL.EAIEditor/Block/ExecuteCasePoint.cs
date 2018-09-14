using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class ExecuteCasePoint : ExecuteSwitchPoint
	{
        public string CaseValue { get; set; }

        public void SetCaseValue(string caseValue)
        {
            this.CaseValue = caseValue;
        }
    }
}