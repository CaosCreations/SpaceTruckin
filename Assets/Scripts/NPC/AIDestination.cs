using System;
using System.Collections.Generic;
using UnityEngine;

public class AIDestination : MonoBehaviour
{
    // The object that determines the vertices of the roaming boundary.
    public GameObject boundingPlane;

    // The bounding area within which the destination can be set.  
    private List<Vector3> boundaryVertices; 

    private void Start()
    {
        Vector3[] vertices = boundingPlane.GetComponent<MeshFilter>().sharedMesh.vertices;
        boundaryVertices = GetBoundaryVertices(vertices);
    }

    private List<Vector3> GetBoundaryVertices(Vector3[] vertices)
    {
        List<Vector3> boundaryVertices = new List<Vector3>();

        // Unity planes have an 11x11 grid of points.
        // We need to get the world position via TransformPoint.
        // The first two indices are the near corners, and the 
        // last two are the far corners (depending on perspective). 
        boundaryVertices.Add(transform.TransformPoint(vertices[0]));
        boundaryVertices.Add(transform.TransformPoint(vertices[10]));
        boundaryVertices.Add(transform.TransformPoint(vertices[110]));
        boundaryVertices.Add(transform.TransformPoint(vertices[120]));

        Debug.Log("Local scale: " + boundingPlane.transform.localScale);

        // If the plane has been scaled up or down, the vertices will need to 
        // be adjusted accordingly, as they are based on the default plane mesh 
        if (boundingPlane.transform.localScale != Vector3.one)
        {
            if (boundaryVertices.Count > 0)
            {
                for (int i = 0; i < boundaryVertices.Count; i++)
                {
                    boundaryVertices[i] = new Vector3(
                        boundaryVertices[i].x * boundingPlane.transform.localScale.x,
                        boundaryVertices[i].y * boundingPlane.transform.localScale.y,
                        boundaryVertices[i].z * boundingPlane.transform.localScale.z);
                }
            }
        }

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

        Debug.Log("1: " + xzPos.Item1 + "\n2: " + xzPos.Item2);

        return xzPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Tuple<float, float> xzPos = GetNextDestination();
            transform.position = new Vector3(xzPos.Item1, transform.position.y, xzPos.Item2);
        }
    }

    private void DrawVertices()
    {
        Gizmos.color = Color.red;
        if (boundaryVertices.Count > 0)
        {
            for (int i = 0; i < boundaryVertices.Count; i++)
            {

                Gizmos.DrawSphere(boundaryVertices[i], 0.25f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawVertices();
    }
}
