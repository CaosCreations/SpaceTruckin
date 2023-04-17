using System;
using UnityEngine;

[Serializable]
public class CutsceneConversationSettings
{
    [field: SerializeField]
    public string ConversationName { get; set; }

    [field: SerializeField]
    public bool CloseDialogueUIOnStart { get; set; }
}