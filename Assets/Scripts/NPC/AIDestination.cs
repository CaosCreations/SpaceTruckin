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

    private Vector2 GetNextDestination()
    {
        // The x and z values of the next destination's position.
        Vector2 xzPosition = new Vector2(
            UnityEngine.Random.Range(
                aiBoundingPlane.BoundaryVertices[0].x, aiBoundingPlane.BoundaryVertices[1].x),
            UnityEngine.Random.Range(
                aiBoundingPlane.BoundaryVertices[0].z, aiBoundingPlane.BoundaryVertices[2].z));

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
        Vector2 xzPosition = GetNextDestination();
        transform.position = new Vector3(xzPosition.x, transform.position.y, xzPosition.y);
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
