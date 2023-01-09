using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#region Enums
public enum RepairsMinigameType
{
    Wheel, Stack, Tile, Simon
}

public enum RepairsMinigameButton
{
    A, B
}
#endregion

public class RepairsMinigamesManager : MonoBehaviour, IRepairsMinigamesManager
{
    public static RepairsMinigamesManager Instance { get; private set; }

    public static event Action<bool> OnMinigameAttemptFinished;

    // Todo: Store references only here
    public Button ButtonA;
    public Button ButtonB;

    #region Init
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

    private void Start()
    {
        // Todo: Change execution order

        //ButtonA = GetRepairsMinigameButton(RepairsMinigameButton.A);
        //ButtonB = GetRepairsMinigameButton(RepairsMinigameButton.B);
    }
    #endregion

    public GameObject InitMinigame(RepairsMinigameType minigameType, Transform parent)
    {
        GameObject repairsMinigameInstance = MinigamePrefabManager.Instance.InitPrefab(minigameType, parent);
        SetButtonVisibility(minigameType);
        repairsMinigameInstance.SetLayerRecursively(UIConstants.RepairsMinigameLayer); ;
        return repairsMinigameInstance;
    }

    // Temporary workaround 
    public static void FinishMinigameAttempt(bool isSuccessfulAttempt)
    {
        OnMinigameAttemptFinished?.Invoke(isSuccessfulAttempt);
    }

    public static void SetButtonVisibility(RepairsMinigameType minigameType)
    {
        var buttonA = GetRepairsMinigameButton(RepairsMinigameButton.A);
        var buttonB = GetRepairsMinigameButton(RepairsMinigameButton.B);

        switch (minigameType)
        {
            case RepairsMinigameType.Wheel:
                buttonA.SetActive(true);
                buttonB.SetActive(false);
                break;
            case RepairsMinigameType.Stack:
                // Stack requires 2 buttons 
                buttonA.SetActive(true);
                buttonB.SetActive(true);
                break;
            default:
                break;
        }
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

    public static RepairsMinigameType GetMinigameTypeByDamageType(ShipDamageType damageType)
    {
        return damageType switch
        {
            ShipDamageType.Engine => RepairsMinigameType.Wheel,
            ShipDamageType.Hull => RepairsMinigameType.Stack,
            _ => throw new ArgumentOutOfRangeException(),
        };

        // Todo: Better default handling here
    }
    #endregion
}
