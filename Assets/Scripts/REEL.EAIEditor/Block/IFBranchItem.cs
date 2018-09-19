using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    using IFBranchParamType = BranchCondition.IFBranchParamType;
    using IFBranchOpParamType = BranchCondition.IFBranchOpParamType;

    public class IFBranchItem : GraphItem
    {
        [SerializeField] private BranchCondition ifBranchData = null;

        [SerializeField] private ExecuteIFPoint executeTrue;
        [SerializeField] private ExecuteIFPoint executeFalse;

        private Dictionary<IFBranchOpParamType, string> opTypeConverterToString = new Dictionary<IFBranchOpParamType, string>();
        private Dictionary<string, IFBranchOpParamType> opTypeConverterToType = new Dictionary<string, IFBranchOpParamType>();

        protected override void Init()
        {
            base.Init();

            opTypeConverterToString.Add(IFBranchOpParamType.Equal, "==");
            opTypeConverterToString.Add(IFBranchOpParamType.NotEqual, "!=");
            opTypeConverterToString.Add(IFBranchOpParamType.Greater, ">");
            opTypeConverterToString.Add(IFBranchOpParamType.GreaterOrEqual, ">=");
            opTypeConverterToString.Add(IFBranchOpParamType.Less, "<");
            opTypeConverterToString.Add(IFBranchOpParamType.LessOrEqual, "<=");

            opTypeConverterToType.Add("==", IFBranchOpParamType.Equal);
            opTypeConverterToType.Add("!=", IFBranchOpParamType.NotEqual);
            opTypeConverterToType.Add(">", IFBranchOpParamType.Greater);
            opTypeConverterToType.Add(">=", IFBranchOpParamType.GreaterOrEqual);
            opTypeConverterToType.Add("<", IFBranchOpParamType.Less);
            opTypeConverterToType.Add("<=", IFBranchOpParamType.LessOrEqual);
        }

        public XMLIFNode GetXMLIFData()
        {
            XMLIFNode xmlNode = new XMLIFNode();
            xmlNode.nodeID = BlockID;
            xmlNode.nodeType = NodeType.IF;
            xmlNode.nodeTitle = GetBlockTitle;

            xmlNode.ifProperty = new IFProperty();
            xmlNode.ifProperty.ifType = ifBranchData.lParamType;
            xmlNode.ifProperty.name = GetBlockName;
            xmlNode.ifProperty.operatorValue = opTypeConverterToString[ifBranchData.opParameter];
            xmlNode.ifProperty.compareValue = ifBranchData.rParameter;
            xmlNode.ifProperty.trueValue = new IFTrue();
            xmlNode.ifProperty.trueValue.nextID = executeTrue.GetLineData != null ? executeTrue.GetLineData.GetRightExecutePointInfo.blockID : -1;

            xmlNode.ifProperty.falseValue = new IFFalse();
            xmlNode.ifProperty.falseValue.nextID = executeFalse.GetLineData != null ? executeFalse.GetLineData.GetRightExecutePointInfo.blockID : -1;

            return xmlNode;
        }

        public string GetStringFromOpType(IFBranchOpParamType type)
        {
            return opTypeConverterToString[type];
        }

        public IFBranchOpParamType GetOpTypeFromString(string type)
        {
            return opTypeConverterToType[type];
        }

        public void SetBlockName(string name)
        {
            ifBranchData.nameField = name;
        }

        public string GetBlockName { get { return ifBranchData.nameField; } }

        public void SetLParamType(int type)
        {
            ifBranchData.lParamType = (IFBranchParamType)type;
        }

        public void SetOpParamType(int type)
        {
            ifBranchData.opParameter = (IFBranchOpParamType)type;
        }

        public void SetOpParamType(string type)
        {
            ifBranchData.opParameter = (IFBranchOpParamType)Enum.Parse(typeof(IFBranchOpParamType), type);
        }

        public void SetRParamValue(string value)
        {
            ifBranchData.rParameter = value;
        }

        public void SetParameters(BranchCondition branchCondition)
        {
            if (ifBranchData == null) ifBranchData = new BranchCondition(branchCondition);
            else ifBranchData.SetValues(branchCondition);
        }

        public BranchCondition GetIFBranchData()
        {
            return ifBranchData;
        }

        public ExecuteIFPoint GetTrueExecutePoint { get { return executeTrue; } }
        public ExecuteIFPoint GetFalseExecutePoint { get { return executeFalse; } }
    }
}