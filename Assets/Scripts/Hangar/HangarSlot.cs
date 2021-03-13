using UnityEngine;

[System.Serializable]
public class HangarSlot : MonoBehaviour // Or make a separate class for HangarSlotInstance
{
    private readonly int node;
    private Ship ship;
    private ShipInstance shipInstance;
    private bool isUnlocked;

    public static string FOLDER_NAME = "HangarSaveData";
    public static string FILE_NAME = "HangarSlots"; // Save all slots in one file

    public void LaunchShip()
    {
        if (shipInstance != null)
        {
            shipInstance.Launch();
        }
    }

    public int Node => node;
    public Ship Ship { get => ship; set => ship = value; }
    public ShipInstance ShipInstance { get => shipInstance; set => shipInstance = value; }
    //public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    public bool IsUnlocked { get => Node <= LicencesManager.HangarSlotUnlockEffect; }
    public bool IsOccupied { get => Ship != null; }
}
