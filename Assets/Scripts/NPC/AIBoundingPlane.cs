using System.Collections.Generic;
using UnityEngine;

public class AIBoundingPlane : MonoBehaviour
{
    // The object that determines the vertices of the bounding area.

    // The corners of the bounding area within which AI destinations can be set.  
    public List<Vector3> boundaryVertices;

    private void Awake()
    {
        Vector3[] vertices = GetComponent<MeshFilter>().sharedMesh.vertices;
        boundaryVertices = GetBoundaryVertices(vertices);
    }

    /// <summary>
    ///     Unity planes have an 11x11 grid of points.
    ///     We need to get the world position via TransformPoint.
    ///     The first two elements are the near corners, and the 
    ///     last two are the far corners (depending on perspective).
    /// </summary>
    /// <param name="vertices"> The raw vertices of the plane's mesh. </param>
    /// <returns> The four corners of the bounding plane. </returns>
    private List<Vector3> GetBoundaryVertices(Vector3[] vertices)
    {
        List<Vector3> boundaryVertices = new List<Vector3>();
        boundaryVertices.Add(transform.TransformPoint(vertices[0]));
        boundaryVertices.Add(transform.TransformPoint(vertices[10]));
        boundaryVertices.Add(transform.TransformPoint(vertices[110]));
        boundaryVertices.Add(transform.TransformPoint(vertices[120]));

        foreach (Vector3 v in boundaryVertices)
        {
            Debug.Log("Vert: " + v);
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
