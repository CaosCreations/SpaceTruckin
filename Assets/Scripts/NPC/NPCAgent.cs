using UnityEngine;
using UnityEngine.AI;

public class NPCAgent: MonoBehaviour
{
    public GameObject Destination;
    private NavMeshAgent agent;
    
    public bool IsWaiting;
    [SerializeField] private float waitTimeLowerBound = 1f;
    [SerializeField] private float waitTimeUpperBound = 4f;

    private float timer;
    
    public Color gizmoColour; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Destination.transform.position);
        timer = GetTimeToWait();
    }

    private float GetTimeToWait()
    {
        return Random.Range(waitTimeLowerBound, waitTimeUpperBound);
    }

    public void Wait() => IsWaiting = true;

    private void Update()
    {
        if (PlayerManager.IsPaused)
        {
            return;
        }

        if (IsWaiting)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = GetTimeToWait();
                IsWaiting = false;
                agent.SetDestination(Destination.transform.position);
            }
        }
    }
}
