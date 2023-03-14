using UnityEngine;

[CreateAssetMenu(fileName = "NPCDataContainer", menuName = "ScriptableObjects/NPCDataContainer", order = 1)]
public class NPCDataContainer : ScriptableObject, IScriptableObjectContainer<NPCData>
{
    [field: SerializeField]
    public NPCData[] Elements { get; set; }
}