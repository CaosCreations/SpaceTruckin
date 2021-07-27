using UnityEngine;

[CreateAssetMenu(fileName = "MissionBonus", menuName = "ScriptableObjects/Missions/Bonuses/MissionBonusContainer", order = 1)]
public class MissionBonusContainer : ScriptableObject
{
    [field: SerializeField]
    public MissionBonus[] MissionBonuses { get; set; }
}
