using System;
using UnityEngine;

public class AIDestination : MonoBehaviour
{
    // The NPC that will move towards the destination.
    public NPCAgent npcAgent; 

    // Determines the area within which the destination can be set. 
    private AIBoundingPlane aiBoundingPlane; 

    private BoxCollider boxCollider;

    private void Start()
    {
        aiBoundingPlane = GetComponentInParent<AIBoundingPlane>();
        boxCollider = GetComponent<BoxCollider>();
        MoveDestination();
    }

    private Tuple<float, float> GetNextDestination()
    {
        // The x and z values of the next destination's position.
        Tuple<float, float> xzPosition = new Tuple<float, float>(
            UnityEngine.Random.Range(
                aiBoundingPlane.boundaryVertices[0].x, aiBoundingPlane.boundaryVertices[1].x),
            UnityEngine.Random.Range(
                aiBoundingPlane.boundaryVertices[0].z, aiBoundingPlane.boundaryVertices[2].z));

        return xzPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // We don't compare tag in case the wrong NPC collides with us.
        if (other.gameObject == npcAgent.gameObject)
        {
            MoveDestination();
            npcAgent.Wait();
        }
    }

    private void MoveDestination()
    {
        Tuple<float, float> xzPosition = GetNextDestination();
        transform.position = new Vector3(xzPosition.Item1, transform.position.y, xzPosition.Item2);
    }

    private void DrawDestination()
    {
        Gizmos.color = npcAgent.gizmoColour;
        if (boxCollider != null)
        {
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }
    }

    private void OnDrawGizmos()
    {
        DrawDestination();
    }
}
