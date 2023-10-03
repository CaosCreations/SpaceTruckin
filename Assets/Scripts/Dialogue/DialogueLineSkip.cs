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
        if (!DialogueManager.IsConversationActive || !dialogueUI.IsActive())
            return;

        if (IsSkipLineInput())
        {
            dialogueUI.OnContinueConversation();
        }

        if (IsSkipConvoInput())
        {
            var entry = DialogueUtils.GetCurrentEntry();
            var conversation = DialogueUtils.GetConversationById(entry.conversationID);
            Debug.Log($"Skipping convo {conversation.Title} at node {entry.id}");

            var seenVarName = DialogueUtils.GetSeenVariableName(conversation);
            if (seenVarName != null)
            {
                DialogueDatabaseManager.Instance.UpdateDatabaseVariable(seenVarName, true);
            }
            DialogueManager.StopAllConversations();

            var accessName = DialogueUtils.GetAccessVariableName(conversation);
            if (accessName != null)
            {
                DialogueDatabaseManager.Instance.UpdateDatabaseVariable(accessName, true);
            }
            DialogueManager.StopAllConversations();
        }
    }

    private bool IsSkipLineInput()
    {
        return keyCodes.Any(key => Input.GetKeyDown(key)) || mouseButtonCodes.Any(btn => Input.GetMouseButtonDown(btn));
    }

    private bool IsSkipConvoInput()
    {
        return Input.GetKey(PlayerConstants.PrototypingModifier) && Input.GetKeyDown(PlayerConstants.SkipConvoKey);
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