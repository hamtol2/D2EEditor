using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using REEL.EAIEditor;
using System.Xml.Serialization;
using System.IO;

namespace RonnieJ.Test
{
	public class XMLTester : MonoBehaviour
	{
        public string path = "test.xml";
        public XMLProject loadedProject;

        private void Start()
        {
            Save();
            //Load();
        }

        private void Load()
        {
            loadedProject = Util.XMLDeserialize<XMLProject>(Application.dataPath + "/" + path);
        }

        private void Save()
        {
            XMLProject project = new XMLProject();
            XMLNodeBase node = new XMLNodeBase();

            XMLStartNode start = new XMLStartNode("0", "Start", "", 1);
            XMLSTTNode stt = new XMLSTTNode("1", "영화 추천 요청", "오늘 영화 뭐 볼까?", 2);
            XMLSwitchNode xmlSwitch = new XMLSwitchNode("6", "Genre", 0);
            xmlSwitch.switchValue = new XMLSwitch();

            project.AddNode(start);
            project.AddNode(stt);
            project.AddNode(xmlSwitch);

            Util.XMLSerialize<XMLProject>(project, Application.dataPath + "/test.xml");
        }
	}
}