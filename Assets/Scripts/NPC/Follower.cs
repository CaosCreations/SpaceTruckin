﻿using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Quaternion initialRotation;

    [SerializeField]
    private float avoidanceDistance = 2.0f;


    //animator Test
    public Animator lilAnim;
   

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lilAnim = GetComponent<Animator>();
        playerTransform = PlayerManager.PlayerObject.transform;
        initialRotation = transform.rotation;
        agent.updateRotation = false;
    }

    private void Update()
    {
      

        Vector3 targetPosition = playerTransform.position - PlayerManager.PlayerMovement.PlayerFacingDirection.normalized * agent.stoppingDistance;

        // Check if the target position is obstructed by walls
        if (Physics.Linecast(playerTransform.position, targetPosition, out RaycastHit hit))
        {
            // If obstructed, find a new valid position using pathfinding
            Vector3 avoidanceDirection = (hit.point - playerTransform.position).normalized;
            Vector3 obstacleAvoidancePosition = hit.point + avoidanceDirection * avoidanceDistance;

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
            // Check if the follower is blocking the player's path
            if (Physics.Linecast(playerTransform.position, targetPosition, out RaycastHit obstructHit))
            {
                // Move around the player if obstructed
                Vector3 avoidanceDirection = (obstructHit.point - playerTransform.position).normalized;
                Vector3 obstacleAvoidancePosition = obstructHit.point + avoidanceDirection * avoidanceDistance;

                if (NavMesh.SamplePosition(obstacleAvoidancePosition, out NavMeshHit navHit, 10.0f, NavMesh.AllAreas))
                {
                    targetPosition = navHit.position;
                    agent.SetDestination(targetPosition);
                }
            }
            else
            {
                // Stop moving if not obstructed
                agent.velocity = Vector3.zero;
            }
        }

        
		 if(playerTransform.transform.position.z>this.transform.position.z+1)
		 {
            lilAnim.SetFloat("Zdirection", -1);
            lilAnim.SetBool("goingUp",true);

		 }

          if(playerTransform.transform.position.z<=this.transform.position.z-1)
		 {
            lilAnim.SetFloat("Zdirection", 1);
            lilAnim.SetBool("goingUp",false);

		 }

          if(playerTransform.transform.position.x>this.transform.position.x)
		 {
            lilAnim.SetFloat("Xdirection", 1);
            lilAnim.SetBool("goingRight",true);

		 }

          if(playerTransform.transform.position.x<=this.transform.position.x)
		 {
            lilAnim.SetFloat("Xdirection", -1);
             lilAnim.SetBool("goingRight",false);

		 }

        // Lock rotation 
        transform.rotation = initialRotation;





    }
}
