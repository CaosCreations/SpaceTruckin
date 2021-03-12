using UnityEngine;

[System.Serializable]
public class HangarSlot : MonoBehaviour // Or make a separate class for HangarSlotInstance
{
    private readonly int node;
    private Ship ship;
    private ShipInstance shipInstance;
    public bool isUnlocked;

    public static string FOLDER_NAME = "HangarSaveData";
    public static string FILE_NAME = "HangarSlots"; // Save all slots in one file

    public HangarSlot(int node, bool isUnlocked)
    {
        this.node = node;
        this.isUnlocked = isUnlocked;
    }

    public void LaunchShip()
    {
        if (shipInstance != null)
        {
            shipInstance.Launch();
        }
    }

    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
}
