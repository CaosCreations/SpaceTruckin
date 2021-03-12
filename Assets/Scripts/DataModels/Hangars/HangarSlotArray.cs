using System;
using System.Collections;
using System.Linq;

public enum HangarSlotNumber
{
    One, Two, Three, Four, Five, Six
}

public class HangarSlotArray : IEnumerable
{
    private readonly HangarSlot[] hangarSlots;

    public HangarSlotArray()
    {
        hangarSlots = new HangarSlot[Enum.GetNames(typeof(HangarSlotNumber)).Length];
    }

    public HangarSlot this[HangarSlotNumber key]
    {
        get => hangarSlots[Convert.ToInt16(key)];
        set => hangarSlots[Convert.ToInt16(key)] = value;
    }

    public IEnumerator GetEnumerator()
    {
        return Enum.GetValues(typeof(HangarSlotNumber))
            .Cast<HangarSlotNumber>()
            .Select(i => this[i]).GetEnumerator();
    }
}
