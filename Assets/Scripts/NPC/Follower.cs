using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Quaternion initialRotation;

    [SerializeField]
    private float avoidanceDistance = 2.0f;

    [SerializeField]
    private float distanceFromPlayer = 1.5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = PlayerManager.PlayerObject.transform;
        initialRotation = transform.rotation;
        agent.updateRotation = false;
    }

    private void Update()
    {
        Vector3 targetPosition = playerTransform.position - playerTransform.forward * distanceFromPlayer;

        // Check if the target position is obstructed by walls
        if (Physics.Linecast(playerTransform.position, targetPosition, out RaycastHit hit))
        {
            // If obstructed, find a new valid position using pathfinding
            Vector3 obstacleAvoidanceDirection = (hit.point - playerTransform.position).normalized;
            Vector3 obstacleAvoidancePosition = hit.point + obstacleAvoidanceDirection * avoidanceDistance;

            if (NavMesh.SamplePosition(obstacleAvoidancePosition, out NavMeshHit navHit, 10.0f, NavMesh.AllAreas))
            {
                targetPosition = navHit.position;
            }
            else
            {
                // Fallback behavior if NavMesh sampling fails
                Debug.Log("Failed to find a valid position for the object to move to!");
            }
        }

        agent.SetDestination(targetPosition);

        // Calculate the desired velocity
        Vector3 desiredVelocity = (targetPosition - transform.position).normalized * PlayerManager.PlayerMovement.CurrentSpeed;
        agent.velocity = desiredVelocity;

        // Check if the follower should stop
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= agent.stoppingDistance)
        {
            agent.velocity = Vector3.zero;
        }

        // Lock rotation 
        transform.rotation = initialRotation;
    }
}
