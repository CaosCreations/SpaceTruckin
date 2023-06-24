using PixelCrushers.DialogueSystem;
using System.Linq;
using UnityEngine;

public class DialogueLineSkip : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField]
    private KeyCode[] keyCodes;

    [Header("Mouse: 0,1,2 are valid")]
    [SerializeField]
    private int[] mouseButtonCodes;

    private AbstractDialogueUI dialogueUI;

    private void Start()
    {
        dialogueUI = FindObjectOfType<AbstractDialogueUI>();
    }

    private void Update()
    {
        if (!DialogueManager.IsConversationActive)
            return;

        if (IsSkipInput())
        {
            dialogueUI.OnContinueConversation();
        }
    }

    private bool IsSkipInput()
    {
        return keyCodes.Any(key => Input.GetKeyDown(key))
            || mouseButtonCodes.Any(btn => Input.GetMouseButtonDown(btn));
    }

    private void OnValidate()
    {
        if (mouseButtonCodes.Any(btn => btn < 0 || btn > 2))
        {
            Debug.LogWarning("Invalid mouse button code. Must be either 0, 1, or 2. (Left, right, or middle).");
            mouseButtonCodes = new int[0];
        }
    }
}