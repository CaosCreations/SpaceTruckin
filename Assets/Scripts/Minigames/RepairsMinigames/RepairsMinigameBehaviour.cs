using UnityEngine;

public abstract class RepairsMinigameBehaviour : MonoBehaviour
{
    [field: SerializeField]
    public RepairsMinigameType MinigameType { get; set; }

    public abstract void SetUp();
}