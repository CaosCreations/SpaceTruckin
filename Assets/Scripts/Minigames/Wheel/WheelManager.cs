using System.Linq;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
    private Wheel wheel;
    private GreenZone greenZone;
    private WheelMinigameUI wheelMinigameUI;

    private int consecutiveWins;
    public bool IsRepairing => wheel.IsRotating;

    private void Start()
    {
        wheel = GetComponentInChildren<Wheel>();
        greenZone = GetComponentInChildren<GreenZone>();

        // Todo: Make a better embedded repairs UI prefab structure to get references recursively
        //wheelMinigameUI = GetComponentInParent<WheelMinigameUI>();

        // Todo: Ensure only one instance of a repairs minigame UI (button) exists at one time
        wheelMinigameUI = GameObject
            .FindGameObjectsWithTag(RepairsConstants.RepairsButtonTag)
            .FirstOrDefault()
            .GetComponent<WheelMinigameUI>();

        if (wheelMinigameUI == null)
        {
            Debug.LogError($"{nameof(WheelMinigameUI)} is null in {nameof(WheelManager)}. Cannot find reference.");
        }
        else
        {
            wheelMinigameUI.AddStopStartListener(StopStart);
        }
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