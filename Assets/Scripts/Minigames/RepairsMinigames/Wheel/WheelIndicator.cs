using UnityEngine;

public class WheelIndicator : MonoBehaviour
{
    private WheelManager wheelManager;
    private bool isInsideGreenZone;

    private void Start()
    {
        wheelManager = GetComponentInParent<WheelManager>();
        Wheel.OnRotationStopped += DetermineOutcome;
    }

    public void DetermineOutcome()
    {
        // Expend a tool regardless of whether the player wins
        PlayerManager.Instance.RepairTools = Mathf.Max(0, PlayerManager.Instance.RepairTools - 1);

        if (isInsideGreenZone)
        {
            wheelManager.PlayerWins();
        }
        else
        {
            wheelManager.PlayerLoses();
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
