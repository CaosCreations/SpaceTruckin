using System;
using UnityEngine;

[Serializable]
public class CanvasTutorialPrefabByDate
{
    [field: SerializeField]
    public Date Date { get; set; }

    [field: SerializeField]
    public GameObject Prefab { get; set; }
}