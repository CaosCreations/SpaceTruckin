using UnityEngine;

[CreateAssetMenu(fileName = "MissionContainer", menuName = "ScriptableObjects/MissionContainer", order = 1)]
public class MissionContainer : ScriptableObject, IScriptableObjectContainer<Mission>
{
    [field: SerializeField]
    public Mission[] Elements { get; set; }
}
