using UnityEngine;
using UnityEngine.AI;

public class NPCAgent: MonoBehaviour
{
    public GameObject destination;
    private NavMeshAgent agent;
    
    public bool isWaiting;
    public int waitTimeLowerBound = 240;
    public int waitTimeUpperBound = 720; 
    private int timer;
    
    public Color gizmoColour; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.transform.position);
        timer = GetTimeToWait();
    }

    private int GetTimeToWait()
    {
        return Random.Range(waitTimeLowerBound, waitTimeUpperBound);
    }

    public void Wait()
    {
        isWaiting = true;
    }

    private void Update()
    {
        if (isWaiting)
        {
            timer--; 
            if (timer <= 0)
            {
                timer = GetTimeToWait();
                isWaiting = false;
                agent.SetDestination(destination.transform.position);
            }
        }
    }




}
