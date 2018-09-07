using System;
using System.Xml.Serialization;
using UnityEngine;

namespace REEL.EAIEditor
{
    [Serializable]
	public class NodeBase
	{
        [XmlIgnore]
        public NodeType nodeType;

        [XmlAttribute("type", typeof(string))]
        public string type;

        [XmlAttribute("id", typeof(string))]
        public string id;

        [XmlElement("title", typeof(string))]
        public string title;

        //[XmlElement()]
	}
}