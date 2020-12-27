using System;
using System.Collections.Generic;
using UnityEngine;

public class AIDestination : MonoBehaviour
{
    // The object that determines the vertices of the roaming boundary.
    public GameObject boundingPlane;

    // The bounding area within which the destination can be set.  
    private List<Vector3> boundaryVertices;

    private void Awake()
    {
        Vector3[] vertices = boundingPlane.GetComponent<MeshFilter>().sharedMesh.vertices;
        boundaryVertices = GetBoundaryVertices(vertices);
    }

    /// <summary>
    ///     Unity planes have an 11x11 grid of points.
    ///     We need to get the world position via TransformPoint.
    ///     The first two indices are the near corners, and the 
    ///     last two are the far corners (depending on perspective).
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns>The four corners of the bounding plane</returns>
    private List<Vector3> GetBoundaryVertices(Vector3[] vertices)
    {
        List<Vector3> boundaryVertices = new List<Vector3>();
        Vector3 extents = boundingPlane.GetComponent<MeshFilter>().mesh.bounds.extents;

        boundaryVertices.Add(boundingPlane.transform.TransformPoint(vertices[0]));
        boundaryVertices.Add(boundingPlane.transform.TransformPoint(vertices[10]));
        boundaryVertices.Add(boundingPlane.transform.TransformPoint(vertices[110]));
        boundaryVertices.Add(boundingPlane.transform.TransformPoint(vertices[120]));

        foreach (Vector3 v in boundaryVertices)
        {
            Debug.Log("Vert: " + v);
        }
        
        return boundaryVertices;
    }

    private Tuple<float, float> GetNextDestination()
    {
        // The x and z values of the next destination's position 
        Tuple<float, float> xzPos = new Tuple<float, float>(
            UnityEngine.Random.Range(boundaryVertices[0].x, boundaryVertices[1].x),
            UnityEngine.Random.Range(boundaryVertices[0].z, boundaryVertices[2].z));

        Debug.Log("New x: " + xzPos.Item1 + "\nNew z: " + xzPos.Item2);

        return xzPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Tuple<float, float> xzPos = GetNextDestination();
            transform.position = new Vector3(xzPos.Item1, transform.position.y, xzPos.Item2);

            NPCAgent agent = other.GetComponent<NPCAgent>();
            if (agent != null)
            {
                agent.isWaiting = true; 
            }
        }
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
