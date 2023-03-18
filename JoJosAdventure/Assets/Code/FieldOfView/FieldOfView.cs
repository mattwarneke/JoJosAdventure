using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class FieldOfView : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        float fieldOfView = 90f;
        Vector3 origin = Vector3.zero;
        int rayCount = 2;
        float currentAngle = 0f;
        float angleIncrease = fieldOfView / rayCount;
        float viewDistance = 50f;

        // plus origin + 3 indexes
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        // polygon
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 1; i <= rayCount; i++)
        {
            Vector3 vertex = origin
                + UtilsClass.GetVectorFromAngle(currentAngle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            currentAngle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}