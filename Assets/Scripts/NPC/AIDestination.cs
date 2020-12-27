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
        boundaryVertices.Add(transform.TransformPoint(vertices[0]));
        boundaryVertices.Add(transform.TransformPoint(vertices[10]));
        boundaryVertices.Add(transform.TransformPoint(vertices[110]));
        boundaryVertices.Add(transform.TransformPoint(vertices[120]));
        
        return boundaryVertices;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            float xPos = Random.Range(1, 200);
            float zPos = Random.Range(1, 200);
            transform.position = new Vector3(xPos, transform.position.y, zPos);
        }
    }
}
