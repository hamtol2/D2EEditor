using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    [System.Serializable]
    public class BranchCondition
    {
        public enum IFBranchParamType
        {
            Variable, Value, Other, None
        }

        public enum IFBranchOpParamType
        {
            Equal, NotEqual, Greater, GreaterOrEqual, Less, LessOrEqual
        }

        public IFBranchParamType lParamType = IFBranchParamType.None;
        public IFBranchParamType rParamType = IFBranchParamType.None;

        public string lParameter = string.Empty;
        public IFBranchOpParamType opParameter;
        public string rParameter = string.Empty;
        public string nameField = string.Empty;

        public BranchCondition(string name, int lType, int rType, string lParam, int opParam, string rParam)
        {
            SetValues(name, lType, rType, lParam, opParam, rParam);
        }

        public BranchCondition(BranchCondition branchCondition)
        {
            SetValues(branchCondition);
        }

        public void SetValues(string name, int lType, int rType, string lParam, int opParam, string rParam)
        {
            nameField = name;
            lParamType = (IFBranchParamType)lType;
            rParamType = (IFBranchParamType)rType;
            lParameter = lParam;
            opParameter = (IFBranchOpParamType)opParam;
            rParameter = rParam;
        }

        public void SetValues(BranchCondition branchCondition)
        {
            nameField = branchCondition.nameField;
            lParamType = branchCondition.lParamType;
            rParamType = branchCondition.rParamType;
            lParameter = branchCondition.lParameter;
            opParameter = branchCondition.opParameter;
            rParameter = branchCondition.rParameter;
        }
    }
}