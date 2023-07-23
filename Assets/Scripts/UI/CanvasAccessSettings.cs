using System;
using UnityEngine;

[Serializable]
public class CanvasAccessSettings
{
    [field: SerializeField]
    public string DialogueVariableName { get; private set; }

    [field: SerializeField]
    public UICanvasType CanvasType { get; private set; }

    [field: SerializeField]
    public string Text { get; private set; }
}
