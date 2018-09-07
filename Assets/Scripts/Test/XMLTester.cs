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
            loadedProject = XMLDeserialize<XMLProject>(Application.dataPath + "/" + path);
        }

        private void Save()
        {
            XMLProject project = new XMLProject();
            XMLNodeBase node = new XMLNodeBase();

            XMLStart start = new XMLStart("START", "0", "Start", "", 1, new Vector2(3.14f, 0.56f));
            XMLSTT stt = new XMLSTT("STT", "1", "영화 추천 요청", "오늘 영화 뭐 볼까?", 2);

            project.AddNode(start);
            project.AddNode(stt);

            //for (int ix = 0; ix < 10; ++ix)
            //{
            //    project.AddNode(node);
            //}

            XMLSerialize<XMLProject>(project, Application.dataPath + "/test.xml");
        }

        void XMLSerialize<T>(T node, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, node);
            writer.Close();
        }

        T XMLDeserialize<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(path);
            T xmlObject = (T)serializer.Deserialize(reader.BaseStream);
            reader.Close();
            return xmlObject;
        }
	}
}