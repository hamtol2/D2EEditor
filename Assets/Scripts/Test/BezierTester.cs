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

        private int curveCount = 0;
        private int layerOrder = 0;
        [SerializeField] private int SEGMENT_COUNT = 50;

        private List<Vector3> bezierPoints = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();

        void Awake()
        {
            if (!lineRenderer)
                lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.sortingLayerID = layerOrder;
            lineRenderer.positionCount = SEGMENT_COUNT + 1;
        }

        private void OnEnable()
        {
            CalculateBezierPoints();
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
                Vector3 point2 = new Vector3(endPoint.position.x, startPoint.position.y);
                Vector3 point3 = new Vector3(startPoint.position.x, endPoint.position.y);
                Vector3 pixel = Util.CalculateCubicBezierPoint(t, startPoint.position, point2, point3, endPoint.position);

                bezierPoints.Add(pixel);
            }
        }

        void CalculateNormals()
        {
            for (int ix = 0; ix < bezierPoints.Count - 1; ++ix)
            {
                Vector3 dir = bezierPoints[ix + 1] - bezierPoints[ix];
                Vector3 normal = new Vector3(dir.y, -dir.x, 0f);
                normals.Add(normal);
            }
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