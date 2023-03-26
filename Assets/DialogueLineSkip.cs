using PixelCrushers.DialogueSystem;
using System.Linq;
using UnityEngine;

public class DialogueLineSkip : MonoBehaviour
{
    [SerializeField]
    private KeyCode[] keyCodes;

    private AbstractDialogueUI dialogueUI;

    private void Start()
    {
        dialogueUI = FindObjectOfType<AbstractDialogueUI>();
    }

    private void Update()
    {
        if (!DialogueManager.IsConversationActive)
            return;

        if (keyCodes.Any(key => Input.GetKeyDown(key)))
        {
            dialogueUI.OnContinueConversation();
        }
    }
}