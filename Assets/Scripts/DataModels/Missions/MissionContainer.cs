using UnityEngine;

[CreateAssetMenu(fileName = "MissionContainer", menuName = "ScriptableObjects/MissionContainer", order = 1)]
public class MissionContainer : ScriptableObject
{
    public Mission[] missions;
}
