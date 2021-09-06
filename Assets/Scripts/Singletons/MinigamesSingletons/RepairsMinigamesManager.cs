using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RepairsMinigamesManager : MonoBehaviour
{
    public static RepairsMinigamesManager Instance { get; private set; }

    public static event Action<bool> OnMinigameAttemptFinished;

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

    public static void FinishMinigameAttempt(bool isSuccessfulAttempt)
    {
        OnMinigameAttemptFinished.Invoke(isSuccessfulAttempt);
    }

    // Gets the button currently embedded in the repairs UI
    public static GameObject GetCurrentRepairsButtonObject()
    {
        // Todo: Ensure only one instance of a repairs minigame UI (button) exists at one time
        GameObject repairsButtonObject = GameObject
            .FindGameObjectsWithTag(RepairsConstants.RepairsButtonTag)
            .FirstOrDefault();

        if (!repairsButtonObject)
        {
            Debug.LogError($"{nameof(repairsButtonObject)} cannot be found (caller: {nameof(RepairsMinigamesManager)})");
        }

        return repairsButtonObject;
    }
}
