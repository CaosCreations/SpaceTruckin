using UnityEngine;

[CreateAssetMenu(fileName = "MissionContainer", menuName = "ScriptableObjects/MissionContainer", order = 1)]
public class MissionContainer : ScriptableObject, IScriptableObjectContainer<Mission>
{
    [field: SerializeField]
    public Mission[] Elements { get; set; }

    [field: SerializeField]
    public Mission[] StartingMissions { get; private set; }

    /// <summary>
    /// TODO: Eventually we may want to configure missions by arbitrary date.
    /// </summary>
    [field: SerializeField]
    public Mission[] Day2Missions { get; private set; }
}
