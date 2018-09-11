using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace REEL.EAIEditor
{
    public static class Util
    {
        public static bool CompareTwoStrings(string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.CurrentCultureIgnoreCase);
        }

        public static float GetAngleBetween(Vector2 start, Vector2 end)
        {
            Vector2 target = end - start;
            return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        }

        public static float GetDistanceBetween(Vector2 start, Vector2 end)
        {
            Vector2 target = end - start;
            return target.magnitude;
        }

        public static T ReadFromJson<T>(TextAsset jsonText)
        {
            return SimpleJson.SimpleJson.DeserializeObject<T>(jsonText.text);
        }

        public static string RemoveAllWhiteSpace(string targetString)
        {
            return targetString.Replace(" ", string.Empty);
        }

        // 두 직선이 서로 교차하는지 확인하는 메소드.
        public static bool CheckLineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float ab = CCW(a, b, c) * CCW(a, b, d);
            float cd = CCW(c, d, a) * CCW(c, d, b);

            return ab <= 0f && cd <= 0f;
        }

        // 직선이 드래그 영역에 포함되는지 확인.
        public static bool CheckLineIncluded(GraphLine.LinePoint linePoint, DragInfo dragInfo)
        {
            if (linePoint.start.x > dragInfo.topLeft.x && linePoint.start.x < dragInfo.topLeft.x + dragInfo.width &&
                linePoint.end.x > dragInfo.topLeft.x && linePoint.end.x < dragInfo.topLeft.x + dragInfo.width &&
                linePoint.start.y < dragInfo.topLeft.y && linePoint.start.y > dragInfo.topLeft.y - dragInfo.height &&
                linePoint.end.y < dragInfo.topLeft.y && linePoint.end.y > dragInfo.topLeft.y - dragInfo.height)
            {
                return true;
            }

            return false;
        }

        private static float CCW(Vector2 a, Vector2 b)
        {
            return a.Cross(b);
        }

        private static float CCW(Vector2 p, Vector2 a, Vector2 b)
        {
            return CCW(a - p, b - p);
        }

        // Vector2 확장 메소드 (외적 계산).
        public static float Cross(this Vector2 myVector, Vector2 otherVector)
        {
            return myVector.x * otherVector.y - myVector.y * otherVector.x;
        }

        public static void XMLSerialize<T>(T node, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, node);
            writer.Close();
        }

        public static T XMLDeserialize<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(path);
            T xmlObject = (T)serializer.Deserialize(reader.BaseStream);
            reader.Close();
            return xmlObject;
        }
    }
}