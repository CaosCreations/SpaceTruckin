using UnityEngine;

[CreateAssetMenu(fileName = "MissionBonus", menuName = "ScriptableObjects/Missions/Bonuses/MissionBonusContainer", order = 1)]
public class MissionBonusContainer : ScriptableObject, IScriptableObjectContainer<MissionBonus>
{
    [field: SerializeField]
    public MissionBonus[] Elements { get; set; }
}
