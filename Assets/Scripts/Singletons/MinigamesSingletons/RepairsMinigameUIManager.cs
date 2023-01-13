using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RepairsMinigameUIManager : MonoBehaviour
{
    public static RepairsMinigameUIManager Instance { get; private set; }

    private Button buttonA;
    private Button buttonB;
    private Text outcomeText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        FindReferences();
    }

    private void FindReferences()
    {
        // Get Button A 
        var objButtonA = GameObject.FindGameObjectWithTag(RepairsConstants.RepairsMinigameButtonATag);

        if (objButtonA == null)
            throw new Exception("Repairs Minigame Button 'A' not found. Check the tags are correct.");

        if (!objButtonA.TryGetComponent(out buttonA))
            throw new Exception("Button component on Button 'A' not found");

        // Get Button B
        var objButtonB = GameObject.FindGameObjectWithTag(RepairsConstants.RepairsMinigameButtonBTag);

        if (objButtonB == null)
            throw new Exception("Repairs Minigame Button 'B' not found. Check the tags are correct.");

        if (!objButtonB.TryGetComponent(out buttonB))
            throw new Exception("Button component on Button 'B' not found");

        // Get Outcome Text
        var objOutcomeText = GameObject.FindGameObjectWithTag(RepairsConstants.RepairsMinigameOutcomeTextTag);

        if (objOutcomeText == null)
            throw new Exception("Repairs Outcome Text not found. Check the tags are correct.");

        if (!objOutcomeText.TryGetComponent(out outcomeText))
            throw new Exception("Text component on Outcome Text not found");
    }

    public void AddOnClick(RepairsMinigameButtonType buttonType, UnityAction callback)
    {
        var button = GetButtonByType(buttonType);
        button.AddOnClick(callback);
    }

    public void SetButtonActive(RepairsMinigameButtonType buttonType, bool isActive)
    {
        var button = GetButtonByType(buttonType);
        button.SetActive(isActive);
    }

    public void ResetUI()
    {
        buttonA.onClick.RemoveAllListeners();
        buttonA.SetActive(false);
        buttonB.onClick.RemoveAllListeners();
        buttonB.SetActive(false);
        outcomeText.Clear();
    }

    private Button GetButtonByType(RepairsMinigameButtonType buttonType)
    {
        return buttonType switch
        {
            RepairsMinigameButtonType.A => buttonA,
            RepairsMinigameButtonType.B => buttonB,
            _ => throw new ArgumentException("Invalid button type: " + buttonType),
        };
    }

    public void SetOutcomeText(string content)
    {
        outcomeText.SetText(content);
    }
}