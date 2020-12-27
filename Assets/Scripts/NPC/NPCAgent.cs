using UnityEngine;
using UnityEngine.AI;

public class NPCAgent: MonoBehaviour
{
    public GameObject npcDestination;
    private NavMeshAgent agent; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        agent.SetDestination(npcDestination.transform.position);
        
    }
}
