using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;

namespace RonnieJ.Test
{
    using REEL.EAIEditor;

    [XmlRoot("node"), 
        XmlInclude(typeof(XMLStartNode)), 
        XmlInclude(typeof(XMLSTTNode)), 
        XmlInclude(typeof(XMLSwitchNode)),
        XmlInclude(typeof(XMLEventNode)),
        XmlInclude(typeof(XMLHearingNode)),
        XmlInclude(typeof(XMLSayNode)),
        XmlInclude(typeof(XMLIFNode)),
        XmlInclude(typeof(XMLFacialNode)),
        XmlInclude(typeof(XMLMotionNode)),
        XmlInclude(typeof(XMLVariableNode)),
        XmlInclude(typeof(XMLTTSNode)),
        XmlInclude(typeof(XMLSpeechRecognitionNode))]
    [Serializable]
    public class XMLNodeBase
    {
        [XmlAttribute("type")] public NodeType type = NodeType.START;
        [XmlAttribute("id")] public string id = "7";
        [XmlElement("title")] public string title = "Start";
        [XmlElement("next")] public int nextNodeID = 123;

        public XMLNodeBase() { }
        public XMLNodeBase(string id, string title, int nextNodeID)
        {
            this.id = id;
            this.title = title;
            this.nextNodeID = nextNodeID;
        }

        public XMLNodeBase(NodeType type, string id, string title, int nextNodeID)
        {
            this.type = type;
            this.id = id;
            this.title = title;
            this.nextNodeID = nextNodeID;
        }
    }

    [Serializable]
    public class XMLStartNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue = "";

        public XMLStartNode() { }
        public XMLStartNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            this.type = NodeType.START;
            this.nodeValue = nodeValue;
        }
        public XMLStartNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLSTTNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLSTTNode() { }
        public XMLSTTNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            this.type = NodeType.STT;
            this.nodeValue = nodeValue;
        }

        public XMLSTTNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLProject
    {
        [XmlArray("xml"), XmlArrayItem("node", typeof(XMLNodeBase))]
        public List<XMLNodeBase> nodes;

        public void AddNode(XMLNodeBase newNode)
        {
            if (nodes == null) nodes = new List<XMLNodeBase>();
            nodes.Add(newNode);
        }
    }

    [Serializable]
    public class XMLEventNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLEventNode() { }
        public XMLEventNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.EVENT;
            this.nodeValue = nodeValue;
        }

        public XMLEventNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLHearingNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLHearingNode() { }
        public XMLHearingNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLHearingNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLSayNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLSayNode() { }
        public XMLSayNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLSayNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLIFNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLIFNode() { }
        public XMLIFNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLIFNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLFacialNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLFacialNode() { }
        public XMLFacialNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLFacialNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLMotionNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLMotionNode() { }
        public XMLMotionNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLMotionNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLVariableNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLVariableNode() { }
        public XMLVariableNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLVariableNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLTTSNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLTTSNode() { }
        public XMLTTSNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLTTSNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [Serializable]
    public class XMLSpeechRecognitionNode : XMLNodeBase
    {
        [XmlElement("value")] public string nodeValue;

        public XMLSpeechRecognitionNode() { }
        public XMLSpeechRecognitionNode(string id, string title, string nodeValue, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.HEARING;
            this.nodeValue = nodeValue;
        }

        public XMLSpeechRecognitionNode(NodeType type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }


    [Serializable]
    public class XMLSwitchNode : XMLNodeBase
    {
        [XmlElement("switch")] public XMLSwitch switchValue;
        [XmlAttribute("next"), XmlIgnore] public string nextNodeID;

        public XMLSwitchNode() { }
        public XMLSwitchNode(string id, string title, int nextNodeID)
            : base(id, title, nextNodeID)
        {
            type = NodeType.SWITCH;
        }

        public XMLSwitchNode(NodeType type, string id, string title, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {

        }
    }

    [Serializable]
    public class XMLSwitch
    {
        [XmlAttribute("type")] public NodeType type = NodeType.STT;
        [XmlAttribute("name")] public string name = "";

        [XmlArray("switch"), XmlArrayItem("case", typeof(XMLSwitchCase)), XmlArrayItem("default", typeof(XMLSwitchDefault))]
        public List<XMLCaseDefaultBase> switchCase = new List<XMLCaseDefaultBase>()
        {
            new XMLSwitchCase(XMLCaseDefaultBase.CaseOrDefault.Case) { caseValue = "액션", nextID = "7" },
            new XMLSwitchCase(XMLCaseDefaultBase.CaseOrDefault.Case) { caseValue = "멜로", nextID = "8" },
            new XMLSwitchCase(XMLCaseDefaultBase.CaseOrDefault.Case) { caseValue = "판타지", nextID = "9" },
            new XMLSwitchCase(XMLCaseDefaultBase.CaseOrDefault.Case) { caseValue = "코미디", nextID = "10" },
            new XMLSwitchDefault(XMLCaseDefaultBase.CaseOrDefault.Default) { nextID = "11" }
        };
    }

    [Serializable]
    public class XMLCaseDefaultBase
    {
        public enum CaseOrDefault { Case, Default }
        [XmlIgnore] public CaseOrDefault caseOrDefault = CaseOrDefault.Case;

        public XMLCaseDefaultBase() { }
        public XMLCaseDefaultBase(CaseOrDefault caseOrDefault) { this.caseOrDefault = caseOrDefault; }
    }

    [Serializable]
    public class XMLSwitchCase : XMLCaseDefaultBase
    {
        [XmlAttribute("value")] public string caseValue = "";
        [XmlAttribute("next")] public string nextID;

        public XMLSwitchCase() { }
        public XMLSwitchCase(CaseOrDefault caseOrDefault) : base(caseOrDefault) { }
    }

    [Serializable]
    public class XMLSwitchDefault : XMLCaseDefaultBase
    {
        [XmlAttribute("next")] public string nextID = "14";

        public XMLSwitchDefault() { }
        public XMLSwitchDefault(CaseOrDefault caseOrDefault) : base(caseOrDefault) { }
    }
}