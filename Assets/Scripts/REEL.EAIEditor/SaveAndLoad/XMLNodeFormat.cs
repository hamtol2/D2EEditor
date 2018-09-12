using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

namespace REEL.EAIEditor
{
    [Serializable]
    [XmlRoot("xml")]
    public class XMLProject
    {
        [XmlArray("project"), XmlArrayItem("node", typeof(XMLNodeBase))]
        public List<XMLNodeBase> nodes = new List<XMLNodeBase>();

        public void AddNode(XMLNodeBase node)
        {
            nodes.Add(node);
        }
    }

    [Serializable]
    [XmlInclude(typeof(XMLNode)), XmlInclude(typeof(XMLSwitchNode)), XmlInclude(typeof(XMLVariableNode))]
    public class XMLNodeBase { }

    // for START, STT, TTS, FACIAL, MOTION.
    [Serializable]
	public class XMLNode : XMLNodeBase
    {
        [XmlAttribute("type")] public NodeType nodeType;
        [XmlAttribute("id")] public int nodeID;
        [XmlElement("title")] public string nodeTitle;
        [XmlElement("value")] public string nodeValue;
        [XmlElement("next")] public int nextID;
    }

    [Serializable]
    public class XMLSwitchNode : XMLNodeBase
    {
        [XmlAttribute("type")] public NodeType nodeType = NodeType.SWITCH;
        [XmlAttribute("id")] public int nodeID;
        [XmlElement("title")] public string nodeTitle;
        [XmlElement("switch", typeof(XMLSwitch))] public XMLSwitch xmlSwitch;
    }

    [Serializable]
    public class XMLSwitch
    {
        [XmlAttribute("type")] public NodeType comparerType;
        [XmlAttribute("name")] public string name;
        [XmlArray("switch"), XmlArrayItem("case", typeof(XMLCase)), XmlArrayItem("default", typeof(XMLDefault))]
        public List<XMLSwitchCase> switchCase = new List<XMLSwitchCase>();

        private int maxCount = 5;

        public void AddCaseOrDefault(XMLSwitchCase switchItem)
        {
            if (switchCase.Count >= maxCount) return;

            if (switchItem is XMLDefault)
                switchCase.Add(switchItem);

            else if (switchItem is XMLCase)
            {
                XMLDefault xmlDefault = switchCase[switchCase.Count - 1] as XMLDefault;
                if (xmlDefault != null)
                {
                    switchCase.RemoveAt(switchCase.Count - 1);
                    switchCase.Add(switchItem);
                    switchCase.Add(xmlDefault);
                }
                else if (xmlDefault == null)
                {
                    switchCase.Add(switchItem);
                }
                else { }
            }
            else { }
        }
    }

    [Serializable]
    public class XMLSwitchCase { }

    [Serializable]
    public class XMLCase : XMLSwitchCase
    {
        [XmlAttribute("value")] public string caseValue;
        [XmlAttribute("next")] public int nextID;
    }

    [Serializable]
    public class XMLDefault : XMLSwitchCase
    {
        [XmlAttribute("next")] public int nextID;
    }

    public enum XMLVariableOperatorType { get, set }

    [Serializable]
    public class XMLVariableNode : XMLNodeBase
    {
        [XmlAttribute("type")] public NodeType nodeType = NodeType.VARIABLE;
        [XmlAttribute("id")] public int nodeID;
        [XmlElement("title")] public string nodeTitle;
        [XmlElement("name")] public string nodeName;
        [XmlElement("operator")] public XMLVariableOperatorType operatorType;
        [XmlElement("value")] public string nodeValue;
        [XmlElement("next")] public int nextID;
    }
}