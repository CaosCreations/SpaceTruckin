using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject greenZone;
    private RepairsManager repairsManager;
    private bool isInsideGreenZone; 

    private void Start()
    {
        repairsManager = GetComponentInParent<RepairsManager>();
        Workstation.onRotationStopped += DetermineOutcome; 
    }

    public void DetermineOutcome()
    {
        if (isInsideGreenZone)
        {
            repairsManager.PlayerWins();
        }
        else
        {
            repairsManager.PlayerLoses();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInsideGreenZone = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        isInsideGreenZone = false;
    }
}
