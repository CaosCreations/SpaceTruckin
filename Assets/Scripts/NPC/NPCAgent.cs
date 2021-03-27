using UnityEngine;
using UnityEngine.AI;

public class NPCAgent: MonoBehaviour
{
    public GameObject destination;
    private NavMeshAgent agent;
    
    public bool isWaiting;
    private float waitTimeLowerBound = 1f;
    private float waitTimeUpperBound = 4f;
    private float timer;
    
    public Color gizmoColour; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.transform.position);
        timer = GetTimeToWait();
    }

    private float GetTimeToWait()
    {
        return Random.Range(waitTimeLowerBound, waitTimeUpperBound);
    }

    public void Wait()
    {
        isWaiting = true;
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsPaused)
        {
            return;
        }

        if (isWaiting)
        {
            timer -= Time.deltaTime; 
            if (timer <= 0)
            {
                timer = GetTimeToWait();
                isWaiting = false;
                agent.SetDestination(destination.transform.position);
            }
        }
    }
}
