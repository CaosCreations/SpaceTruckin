using System;
using UnityEngine;

[Serializable]
public class CanvasTutorialByDate
{
    [field: SerializeField]
    public Date Date { get; set; }

    [field: SerializeField]
    public InteractiveCanvasTutorial Tutorial { get; set; }
}