using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum RepairsMinigameButton
{
    A, B
}

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

    #region Find References
    // Gets the button currently embedded in the repairs UI
    public static GameObject GetRepairsMinigameButtonObject(RepairsMinigameButton buttonType)
    {
        string buttonTag = GetTagByButtonType(buttonType);

        GameObject repairsButtonObject = GameObject
            .FindGameObjectsWithTag(buttonTag)
            .FirstOrDefault();

        if (!repairsButtonObject)
        {
            Debug.LogError($"{nameof(repairsButtonObject)} cannot be found (caller: {nameof(RepairsMinigamesManager)})");
        }

        return repairsButtonObject;
    }

    public static Button GetRepairsMinigameButton(RepairsMinigameButton buttonType)
    {
        Button repairsMinigameButton = GetRepairsMinigameButtonObject(buttonType)
            .GetComponent<Button>();

        if (!repairsMinigameButton)
        {
            Debug.LogError($"{nameof(repairsMinigameButton)} cannot be found (caller: {nameof(RepairsMinigamesManager)})");
        }

        return repairsMinigameButton;
    }

    public static Text GetRepairsMinigameFeedbackText()
    {
        Text feedbackText = GameObject
            .FindGameObjectWithTag(RepairsConstants.RepairsMinigameFeedbackTextTag)
            .GetComponent<Text>();

        if (!feedbackText)
        {
            Debug.LogError($"{nameof(feedbackText)} cannot be found (caller: {nameof(RepairsMinigamesManager)})");
        }

        return feedbackText;
    }

    private static string GetTagByButtonType(RepairsMinigameButton buttonType)
    {
        return buttonType switch
        {
            RepairsMinigameButton.A => RepairsConstants.RepairsMinigameButtonATag,
            RepairsMinigameButton.B => RepairsConstants.RepairsMinigameButtonBTag,
            _ => string.Empty,
        };
    }
    #endregion
}
