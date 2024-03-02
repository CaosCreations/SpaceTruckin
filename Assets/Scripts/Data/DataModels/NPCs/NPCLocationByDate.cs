using System;
using UnityEngine;

[Serializable]
public class NPCLocationByDate
{
    [field: SerializeField]
    public Date Date { get; private set; }

    [field: SerializeField]
    public NPCLocation Location { get; private set; }
}