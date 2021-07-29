using UnityEngine;

[CreateAssetMenu(fileName = "MissionContainer", menuName = "ScriptableObjects/MissionContainer", order = 1)]
public class MissionContainer : ScriptableObject, IScriptableObjectContainer<Mission>
{
    //public Mission[] Missions;
    [field: SerializeField]
    public Mission[] Elements { get; set; }
}
