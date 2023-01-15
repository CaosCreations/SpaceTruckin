using UnityEngine.Events;

public static class RepairsMinigameInteraction
{
    // Events 
    public static UnityAction<RepairsMinigameType> OnMinigameStart;
    public static UnityAction<RepairsMinigameType> OnMinigameEnd;
    public static UnityAction<RepairsMinigameType> OnMinigameWon;
    public static UnityAction<RepairsMinigameType> OnMinigameLost;
}
