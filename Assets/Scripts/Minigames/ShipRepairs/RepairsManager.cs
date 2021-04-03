using UnityEngine;

public class RepairsManager : MonoBehaviour
{
    private Workstation workstation;
    private GreenZone greenZone;
    private RepairsUI repairsUI;

    public int consecutiveWins;

    private void Start()
    {
        workstation = GetComponentInChildren<Workstation>();
        greenZone = GetComponentInChildren<GreenZone>();
        repairsUI = GetComponentInParent<RepairsUI>();
    }

    public void PlayerWins()
    {
        workstation.IncreaseRotationSpeed();
        Debug.Log("New speed: " + workstation.currentRotationSpeed);

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