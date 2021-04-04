using UnityEngine;

public class Indicator : MonoBehaviour
{
    private RepairsManager repairsManager;
    private bool isInsideGreenZone; 

    private void Start()
    {
        repairsManager = GetComponentInParent<RepairsManager>();
        Workstation.OnRotationStopped += DetermineOutcome; 
    }

    public void DetermineOutcome()
    {
        // Expend a tool regardless of whether the player wins
        PlayerManager.Instance.RepairTools--;
     
        if (isInsideGreenZone)
        {
            repairsManager.PlayerWins();
        }
        else
        {
            repairsManager.PlayerLoses();
        }

        // Expend a tool regardless of whether the player wins
        PlayerManager.Instance.RepairTools--;
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
