using System.Collections.Generic;
using UnityEngine;

public class AIBoundingPlane : MonoBehaviour
{
    // The corners of the bounding area within which AI destinations can be set.  
    public List<Vector3> boundaryVertices;

    private int[] vertexIndexes = new int[] { 0, 10, 110, 120 };

    private void Awake()
    {
        Vector3[] vertices = GetComponent<MeshFilter>().sharedMesh.vertices;
        boundaryVertices = GetBoundaryVertices(vertices);
    }

    /// <summary>
    ///     Unity planes have an 11x11 grid of points.
    ///     Index 0, 10, 110, and 120 are the four corners of the plane.
    ///     The first two elements are the near corners, and the 
    ///     last two are the far corners (depending on perspective).
    ///     
    ///     We need to get the world position via TransformPoint.
    /// </summary>
    /// <param name="vertices"> The raw vertices of the plane's mesh. </param>
    /// <returns> The four corners of the bounding plane. </returns>
    private List<Vector3> GetBoundaryVertices(Vector3[] vertices)
    {
        List<Vector3> boundaryVertices = new List<Vector3>();

        foreach (int index in vertexIndexes)
        {
            boundaryVertices.Add(transform.TransformPoint(vertices[index]));
        }
        return boundaryVertices;
    }

    private void DrawBoundaryVertices()
    {
        Gizmos.color = Color.red;
        if (boundaryVertices != null && boundaryVertices.Count > 0)
        {
            for (int i = 0; i < boundaryVertices.Count; i++)
            {
                Gizmos.DrawSphere(boundaryVertices[i], 0.25f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawBoundaryVertices();
    }
}
