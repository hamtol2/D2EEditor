using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Bezier : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public LineRenderer lineRenderer;

    private int curveCount = 0;
    private int layerOrder = 0;
    [SerializeField] private int SEGMENT_COUNT = 50;

    void Start()
    {
        if (!lineRenderer)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.sortingLayerID = layerOrder;
        lineRenderer.positionCount = SEGMENT_COUNT + 1;
    }

    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        for (int ix = 0; ix <= SEGMENT_COUNT; ++ix)
        {
            float t = ix / (float)SEGMENT_COUNT;
            Vector3 point2 = new Vector3(endPoint.position.x, startPoint.position.y);
            Vector3 point3 = new Vector3(startPoint.position.x, endPoint.position.y);
            Vector3 pixel = CalculateCubicBezierPoint(t, startPoint.position, point2, point3, endPoint.position);
            lineRenderer.SetPosition(ix, pixel);
        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}