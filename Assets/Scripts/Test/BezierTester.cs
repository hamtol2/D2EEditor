using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using REEL.EAIEditor;

namespace REEL.Test
{
	public class BezierTester : MonoBehaviour
	{
        public Transform startPoint;
        public Transform endPoint;
        public LineRenderer lineRenderer;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        public GameObject bezierPointPrefab;
        public GameObject normalPrefab;

        private int curveCount = 0;
        private int layerOrder = 0;
        [SerializeField] private int SEGMENT_COUNT = 50;

        private List<Vector3> bezierPoints = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();

        void Awake()
        {
            if (!lineRenderer)
                lineRenderer = GetComponent<LineRenderer>();

            //lineRenderer.sortingLayerID = layerOrder;
            //lineRenderer.positionCount = SEGMENT_COUNT + 1;
        }

        private void OnEnable()
        {
            //CalculateBezierPoints();
            DrawMesh();
        }

        void Update()
        {
            //DrawCurve();
        }

        void CalculateBezierPoints()
        {
            for (int ix = 0; ix <= SEGMENT_COUNT; ++ix)
            {
                float t = ix / (float)SEGMENT_COUNT;
                //Vector3 point2 = new Vector3(endPoint.position.x, startPoint.position.y);
                //Vector3 point3 = new Vector3(startPoint.position.x, endPoint.position.y);
                Vector3 point2 = new Vector3(-1.5f, 0f);
                Vector3 point3 = new Vector3(1.5f, 0f);
                Vector3 pixel = Util.CalculateCubicBezierPoint(t, startPoint.position, point2, point3, endPoint.position);

                bezierPoints.Add(pixel);
                GameObject point = Instantiate(bezierPointPrefab);
                point.transform.position = pixel;
                //Debug.Log(pixel);
            }
        }

        List<Vector3> vertices = new List<Vector3>();

        void CalculateNormals()
        {
            for (int ix = 0; ix < bezierPoints.Count - 1; ++ix)
            {
                Vector3 dir = bezierPoints[ix + 1] - bezierPoints[ix];
                //Vector3 normal = new Vector3(dir.y, -dir.x, 0f);
                Vector3 normal = Vector3.Cross(dir, Vector3.forward);
                normal.Normalize();
                normals.Add(normal);

                GameObject normalObj = Instantiate(normalPrefab);
                normalObj.transform.position = normal;
                //Debug.Log(normal);
            }
        }

        void GetVertices()
        {
            for (int ix = 0; ix < normals.Count; ++ix)
            {
                vertices.Add(bezierPoints[ix]);
                vertices.Add(normals[ix]);
            }
        }

        List<int> indices = new List<int>();

        void SetIndices()
        {
            // for the first triangles.
            for (int ix = 0; ix < vertices.Count - 2; ix = ix + 2)
            {
                indices.Add(ix);
                indices.Add(ix + 1);
                indices.Add(ix + 2);
            }

            for (int ix = 1; ix < vertices.Count - 2; ix = ix + 2)
            {
                indices.Add(ix);
                indices.Add(ix + 2);
                indices.Add(ix + 1);
            }
        }

        void DrawMesh()
        {
            CalculateBezierPoints();
            CalculateNormals();
            GetVertices();
            SetIndices();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

            meshFilter.mesh = mesh;
        }

        void DrawCurve()
        {
            for (int ix = 0; ix <= SEGMENT_COUNT; ++ix)
            {
                float t = ix / (float)SEGMENT_COUNT;
                Vector3 point2 = new Vector3(endPoint.position.x, startPoint.position.y);
                Vector3 point3 = new Vector3(startPoint.position.x, endPoint.position.y);
                Vector3 pixel = Util.CalculateCubicBezierPoint(t, startPoint.position, point2, point3, endPoint.position);
                GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cube);
                point.transform.position = pixel;
                point.transform.localScale = Vector2.one * 0.1f;
                //lineRenderer.SetPosition(ix, pixel);
            }
        }
    }
}