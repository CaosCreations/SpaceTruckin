using System;
using UnityEngine;

[Serializable]
public class CutsceneConversationSettings
{
    [field: SerializeField]
    public int StartConversationId { get; private set; }

    [field: SerializeField]
    public int EndConversationId { get; private set; }

    [field: SerializeField]
    public bool CloseDialogueUIOnStart { get; private set; }

    [field: SerializeField]
    public bool ContinueOnEnd { get; private set; }
}
