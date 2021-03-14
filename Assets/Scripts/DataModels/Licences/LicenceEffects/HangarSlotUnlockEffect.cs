using UnityEngine;

[CreateAssetMenu(fileName = "HangarSlotUnlockEffectEffect", menuName = "ScriptableObjects/Licences/HangarSlotUnlockEffect", order = 1)]
public class HangarSlotUnlockEffect : UnlockEffect
{
    [SerializeField] private int numberOfSlotsToUnlock;
    public int NumberOfSlotsToUnlock { get; set; }

}
