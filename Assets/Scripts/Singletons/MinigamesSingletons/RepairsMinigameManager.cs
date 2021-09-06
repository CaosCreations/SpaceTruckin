using System;
using UnityEngine;

public class RepairsMinigameManager : MonoBehaviour
{
    public static RepairsMinigameManager Instance { get; private set; }

    public static event Action<bool> OnMinigameAttemptFinished;

    public static void FinishMinigameAttempt(bool isSuccessfulAttempt)
    {
        OnMinigameAttemptFinished.Invoke(isSuccessfulAttempt);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

}
