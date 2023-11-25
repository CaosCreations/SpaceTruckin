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
    public bool OpenDialogueUIOnEnd { get; private set; }

    [field: SerializeField]
    public bool ContinueOnEnd { get; private set; }

    [field: SerializeField]
    public bool DontPauseDialogueOnStart { get; private set; }

    [field: SerializeField]
    public bool DontUnpauseDialogueOnEnd { get; private set; }

    [field: SerializeField]
    public bool DontUnpausePlayerOnEnd { get; private set; }
}
