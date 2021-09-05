using UnityEngine;

public class WheelManager : MonoBehaviour
{
    private Wheel wheel;
    private GreenZone greenZone;
    //private RepairsUI repairsUI;
    private WheelMinigameUI wheelMinigameUI;

    private int consecutiveWins;
    public bool IsRepairing => wheel.IsRotating;

    private void Start()
    {
        wheel = GetComponentInChildren<Wheel>();
        greenZone = GetComponentInChildren<GreenZone>();

        wheelMinigameUI = GetComponentInParent<WheelMinigameUI>();
        wheelMinigameUI.AddStopStartListener(StopStart);
    }

    private void OnDisable()
    {
        wheel.ResetWheel();
    }

    public void StopStart()
    {
        if (wheel.IsRotating)
        {
            wheel.StopRotating();
        }
        else
        {
            wheel.StartRotating();
        }
    }

    public void PlayerWins()
    {
        ShipsManager.RepairShip();

        wheel.IncreaseRotationSpeed();
        Debug.Log("New speed: " + wheel.CurrentRotationSpeed);

        // Decrease green zone size every n wins 
        if (IsGreenZoneShrinking)
        {
            greenZone.ReduceSize();
        }

        if (IsDirectionReversing)
        {
            wheel.ReverseRotationDirection();
        }

        //repairsUI.UpdateUI(wasSuccessful: true);
    }

    public void PlayerLoses()
    {
        consecutiveWins = 0;
        wheel.ResetRotationSpeed();
        greenZone.ResetSize();
        //repairsUI.UpdateUI(wasSuccessful: false);
    }

    public bool IsGreenZoneShrinking
    {
        get => consecutiveWins % RepairsConstants.GreenZoneShrinkInterval == 0 
            && consecutiveWins > 0;
    }

    public bool IsDirectionReversing
    {
        get => Random.Range(0, RepairsConstants.RotationReversalUpperBound) 
            > RepairsConstants.RotationReversalThreshold;
    }
}