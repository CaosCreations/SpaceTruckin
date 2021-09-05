using UnityEngine;

public class WheelIndicator : MonoBehaviour
{
    private WheelManager repairsManager;
    private bool isInsideGreenZone; 

    private void Start()
    {
        repairsManager = GetComponentInParent<WheelManager>();
        Wheel.OnRotationStopped += DetermineOutcome; 
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
