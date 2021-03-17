using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    [SerializeField] private int node;
    private Ship ship;
    private ShipInstance shipInstance;

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
    public bool IsUnlocked { get => Node <= LicencesManager.HangarSlotUnlockEffect; } // 
    public bool IsOccupied { get => Ship != null; }
}
