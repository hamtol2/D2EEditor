using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace RonnieJ.Test
{
    [XmlRoot("node"), XmlInclude(typeof(XMLStart)), XmlInclude(typeof(XMLSTT))]
    public class XMLNodeBase
    {
        [XmlAttribute("type")] public string type = "START";
        [XmlAttribute("id")] public string id = "7";
        [XmlElement("title", Order = 1)] public string title = "Start";
        [XmlElement("next", Order = 10)] public int nextNodeID = 123;


        public XMLNodeBase() { }
        public XMLNodeBase(string type, string id, string title, int nextNodeID)
        {
            this.type = type;
            this.id = id;
            this.title = title;
            this.nextNodeID = nextNodeID;
        }
    }

    public class XMLStart : XMLNodeBase
    {
        [XmlElement("value", Order = 2)] public string nodeValue = "";
        [XmlElement("position")] public Vector2 position;
        [XmlElement("switch")] public Switch Switch;

        public XMLStart() { }
        public XMLStart(string type, string id, string title, string nodeValue, int nextNodeID, Vector2 position)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
            this.position = position;
        }
    }

    public class XMLSTT : XMLNodeBase
    {
        [XmlElement("value", Order = 2)] public string nodeValue;

        public XMLSTT() { }
        public XMLSTT(string type, string id, string title, string nodeValue, int nextNodeID)
            : base(type, id, title, nextNodeID)
        {
            this.nodeValue = nodeValue;
        }
    }

    [System.Serializable]
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

    [System.Serializable]
    public class Switch
    {
        [XmlAttribute("type")] public string type;
        [XmlAttribute("name")] public string name = "";

        [XmlArray("switch"), XmlArrayItem("case", typeof(SwitchCase))]
        public List<SwitchCase> switchCase = new List<SwitchCase>()
        {
            new SwitchCase() { caseValue = "블랙팬서", nextID = "15" },
            new SwitchCase() { caseValue = "더 그레이", nextID = "16" }
        };

        [XmlElement("default")]
        public SwitchDefault switchDefault;
    }

    [System.Serializable]
    public class SwitchCase
    {
        [XmlAttribute("value")]
        public string caseValue = "";

        [XmlAttribute("next")]
        public string nextID;
    }

    [System.Serializable]
    public class SwitchDefault
    {
        [XmlAttribute("next")]
        public string nextID = "14";
    }
}