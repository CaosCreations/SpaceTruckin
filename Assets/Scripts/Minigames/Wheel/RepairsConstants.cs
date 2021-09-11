public static class RepairsConstants
{
    // Workstation 
    public const float StartingSpeed = 180f;
    public const float SpeedIncrease = 12f;

    // Green zone 
    public const float SizeDecrease = 0.01f;

    // Difficulty increase-related 
    public const float RotationReversalThreshold = 3f;
    public const float RotationReversalUpperBound = 5f;
    public const float GreenZoneShrinkInterval = 3f;

    // UI 
    public const string SuccessMessage = "Success!";
    public const string FailureMessage = "Failure!";
    public const string StartButtonText = "Start Repairing";
    public const string StopButtonText = "Stop Repairing";
    public const string ToolsCostText = "Cost to purchase: $";

    // Tools
    public const int CostPerTool = 10;

    // Gameplay 
    public const int HullRepairedPerWin = 15;

    // Tags
    public const string RepairsMinigameButtonATag = "RepairsMinigameButtonA";
    public const string RepairsMinigameButtonBTag = "RepairsMinigameButtonB";
    public const string RepairsMinigameFeedbackTextTag = "RepairsMinigameFeedbackText";

}
