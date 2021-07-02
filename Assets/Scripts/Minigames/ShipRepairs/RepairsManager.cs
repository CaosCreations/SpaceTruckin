using UnityEngine;

public class RepairsManager : MonoBehaviour
{
    public Workstation workstation;
    private GreenZone greenZone;
    private RepairsUI repairsUI;

    private int consecutiveWins;
    public bool IsRepairing => workstation.IsRotating;

    private void Start()
    {
        workstation = GetComponentInChildren<Workstation>();
        greenZone = GetComponentInChildren<GreenZone>();
        repairsUI = GetComponentInParent<RepairsUI>();
    }

    private void OnDisable()
    {
        workstation.ResetWorkstation();
    }

    public void StopStart()
    {
        if (workstation.IsRotating)
        {
            workstation.StopRotating();
        }
        else
        {
            workstation.StartRotating();
        }
    }

    public void PlayerWins()
    {
        if (repairsUI.ShipToRepair != null)
        {
            ShipsManager.RepairShip(repairsUI.ShipToRepair);
        }

        workstation.IncreaseRotationSpeed();
        Debug.Log("New speed: " + workstation.CurrentRotationSpeed);

        // Decrease green zone size every n wins 
        if (IsGreenZoneShrinking())
        {
            greenZone.ReduceSize();
        }

        if (IsDirectionReversing())
        {
            workstation.ReverseRotationDirection();
        }

        repairsUI.UpdateUI(wasSuccessful: true);
    }

    public void PlayerLoses()
    {
        consecutiveWins = 0;
        workstation.ResetRotationSpeed();
        greenZone.ResetSize();
        repairsUI.UpdateUI(wasSuccessful: false);
    }

    public bool IsGreenZoneShrinking()
    {
        return consecutiveWins % RepairsConstants.GreenZoneShrinkInterval == 0
            && consecutiveWins > 0;
    }

    public bool IsDirectionReversing()
    {
        return Random.Range(0, RepairsConstants.RotationReversalUpperBound)
            > RepairsConstants.RotationReversalThreshold;
    }
}