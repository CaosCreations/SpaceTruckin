using UnityEngine;

public class Hangar : ScriptableObject
{
    [SerializeField] private int hangarNumber;
    [SerializeField] private HangarSlot[] hangarSlots;
    //[SerializeField] private HangarSlotArray hangarSlots;
    [SerializeField] private bool isUnlocked;

    public int Number { get => hangarNumber; }
    public HangarSlot[] HangarSlots { get => hangarSlots; }
    //public HangarSlotArray HangarSlots { get => hangarSlots; }
    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
}