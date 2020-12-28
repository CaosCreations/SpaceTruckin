using UnityEngine;
using UnityEngine.AI;

public class NPCAgent: MonoBehaviour
{
    public GameObject destination;
    private NavMeshAgent agent;
    
    public bool isWaiting;
    [SerializeField] private int waitTimeLowerBound = 240;
    [SerializeField] private int waitTimeUpperBound = 720; 
    private int timer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.transform.position);
        timer = GetTimeToWait();
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

    private int GetTimeToWait()
    {
        return Random.Range(waitTimeLowerBound, waitTimeUpperBound);
    }
}
