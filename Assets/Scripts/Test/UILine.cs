using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.Test
{
	public class UILine : MonoBehaviour
	{
        [SerializeField] private CanvasRenderer canvasRenderer;
        [SerializeField] private Material material;
        private float offset = 1f;
        private float baseValue = 200f;
        private int index = 0;

        private void Awake()
        {
            if (canvasRenderer == null)
                canvasRenderer = GetComponent<CanvasRenderer>();

            SetVerts();
        }

        private void LateUpdate()
        {
            SetVerts();
        }

        void SetVerts()
        {
            Mesh mesh = new Mesh();

            // Move Test.
            //float moveFactor = offset * index++;
            //mesh.vertices = new Vector3[]
            //{
            //    new Vector3(-baseValue, -baseValue) + new Vector3(moveFactor, 0f),
            //    new Vector3(-baseValue, baseValue) + new Vector3(moveFactor, 0f),
            //    new Vector3(baseValue, baseValue) + new Vector3(moveFactor, 0f),
            //    new Vector3(baseValue, -baseValue)  + new Vector3(moveFactor, 0f),
            //};

            // Normal Vertices.
            mesh.vertices = new Vector3[]
            {
                new Vector3(-baseValue, -baseValue),
                new Vector3(-baseValue, baseValue),
                new Vector3(baseValue, baseValue),
                new Vector3(baseValue, -baseValue),
            };

            int[] indices = new int[]
            {
                0, 1, 2, 0, 2, 3
            };

            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.colors = new Color[] { Color.red, Color.red, Color.blue, Color.blue };

            Material pMat = new Material(material);
            pMat.color = Color.blue;
            //canvasRenderer.Clear();
            canvasRenderer.SetMaterial(pMat, null);
            canvasRenderer.SetMesh(mesh);
            canvasRenderer.SetColor(Color.red);
        }
    }
}