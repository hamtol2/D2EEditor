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
            XMLNode start = new XMLNode()
            {
                nodeID = 0,
                nodeType = NodeType.START,
                nodeTitle = "Start",
                nodeValue = "",
                nextID = 1
            };

            project.AddNode(start);

            XMLNode one = new XMLNode()
            {
                nodeID = 1,
                nodeType = NodeType.STT,
                nodeTitle = "영화 추천 요청",
                nodeValue = "오늘 영화 뭐 볼까",
                nextID = 2
            };

            project.AddNode(one);

            XMLNode two = new XMLNode()
            {
                nodeID = 2,
                nodeType = NodeType.TTS,
                nodeTitle = "추천 응답",
                nodeValue = "제가 추천 해 드릴게요",
                nextID = 3
            };

            project.AddNode(two);

            XMLNode three = new XMLNode()
            {
                nodeID = 3,
                nodeType = NodeType.FACIAL,
                nodeTitle = "smile",
                nodeValue = "smile",
                nextID = 4
            };

            project.AddNode(three);

            XMLNode four = new XMLNode()
            {
                nodeID = 4,
                nodeType = NodeType.MOTION,
                nodeTitle = "hi",
                nodeValue = "hi",
                nextID = 5
            };

            project.AddNode(four);

            XMLNode five = new XMLNode()
            {
                nodeID = 5,
                nodeType = NodeType.TTS,
                nodeTitle = "장르 제시",
                nodeValue = "액션 멜로 판타지 코미디 등이 있는데 어떤게 좋으세요?",
                nextID = 6
            };

            project.AddNode(five);

            XMLSwitchNode six = new XMLSwitchNode()
            {
                nodeID = 6,
                nodeType = NodeType.SWITCH,
                nodeTitle = "Genre",
                xmlSwitch = new XMLSwitch()
                {
                    comparerType = NodeType.STT,
                    name = "",
                    switchCase = new List<XMLSwitchCase>()
                    {
                        new XMLCase() { caseValue = "액션", nextID = 7,},
                        new XMLCase() { caseValue = "멜로", nextID = 8,},
                        new XMLCase() { caseValue = "판타지", nextID = 9,},
                        new XMLCase() { caseValue = "코미디", nextID = 10,},
                        new XMLDefault() { nextID = 11,},
                    }
                }
            };

            project.AddNode(six);

            XMLVariableNode seven = new XMLVariableNode()
            {
                nodeID = 7,
                nodeType = NodeType.VARIABLE,
                nodeTitle = "액션 저장",
                nodeName = "genre",
                operatorType = XMLVariableOperatorType.set,
                nodeValue = "액션",
                nextID = 12
            };

            project.AddNode(seven);

            XMLVariableNode eight = new XMLVariableNode()
            {
                nodeID = 8,
                nodeType = NodeType.VARIABLE,
                nodeTitle = "멜로 저장",
                nodeName = "genre",
                operatorType = XMLVariableOperatorType.set,
                nodeValue = "멜로",
                nextID = 13
            };

            project.AddNode(eight);

            XMLVariableNode nine = new XMLVariableNode()
            {
                nodeID = 9,
                nodeType = NodeType.VARIABLE,
                nodeTitle = "판타지 저장",
                nodeName = "genre",
                operatorType = XMLVariableOperatorType.set,
                nodeValue = "판타지",
                nextID = 13
            };

            project.AddNode(nine);

            XMLVariableNode ten = new XMLVariableNode()
            {
                nodeID = 10,
                nodeType = NodeType.VARIABLE,
                nodeTitle = "코미디 저장",
                nodeName = "genre",
                operatorType = XMLVariableOperatorType.set,
                nodeValue = "코미디",
                nextID = 13
            };

            project.AddNode(ten);

            XMLNode eleven = new XMLNode()
            {
                nodeID = 11,
                nodeType = NodeType.TTS,
                nodeTitle = "다시 응답",
                nodeValue = "다시 말씀해주세요",
                nextID = 6
            };

            project.AddNode(eleven);

            XMLNode twelve = new XMLNode()
            {
                nodeID = 12,
                nodeType = NodeType.TTS,
                nodeTitle = "액션 영화 종류",
                nodeValue = "최신 액션 영화에는 블랙팬서, 더 그레이가 있습니다",
                nextID = 14
            };

            project.AddNode(twelve);

            XMLNode thirteen = new XMLNode()
            {
                nodeID = 13,
                nodeType = NodeType.TTS,
                nodeTitle = "액션 영화 종류",
                nodeValue = "(get genre) 영화는 아직 준비되지 않았습니다. 다른 영화를 선택 해 주세요",
                nextID = 14
            };

            project.AddNode(thirteen);

            XMLSwitchNode fourteen = new XMLSwitchNode()
            {
                nodeID = 14,
                nodeType = NodeType.SWITCH,
                nodeTitle = "Movie",
                xmlSwitch = new XMLSwitch()
                {
                    comparerType = NodeType.STT,
                    name = "",
                    switchCase = new List<XMLSwitchCase>()
                    {
                        new XMLCase() { caseValue = "블랙펜서", nextID = 15,},
                        new XMLCase() { caseValue = "더 그레이", nextID = 16,},
                        new XMLDefault() { nextID = 14,},
                    }
                }
            };

            project.AddNode(fourteen);

            XMLVariableNode fifteen = new XMLVariableNode()
            {
                nodeID = 15,
                nodeType = NodeType.VARIABLE,
                nodeTitle = "블랙펜서 저장",
                nodeName = "movie",
                operatorType = XMLVariableOperatorType.set,
                nodeValue = "블랙펜서",
                nextID = 17
            };

            project.AddNode(fifteen);

            XMLVariableNode sixteen = new XMLVariableNode()
            {
                nodeID = 16,
                nodeType = NodeType.VARIABLE,
                nodeTitle = "더 그레이 저장",
                nodeName = "movie",
                operatorType = XMLVariableOperatorType.set,
                nodeValue = "더 그레이",
                nextID = 17
            };

            project.AddNode(sixteen);

            XMLNode seventeen = new XMLNode()
            {
                nodeID = 17,
                nodeType = NodeType.TTS,
                nodeTitle = "영화 상영",
                nodeValue = "그러면 (get genre)장르의 (get movie) 영화를 상영하겠습니다",
                nextID = -1
            };

            project.AddNode(seventeen);

            Util.XMLSerialize<XMLProject>(project, Application.dataPath + "/test.xml");
        }
    }
}