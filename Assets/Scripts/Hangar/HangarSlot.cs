using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    public HangarNode node;
    public ShipInstance shipInstance;

    public void LaunchShip()
    {
        if (shipInstance != null)
        {
            shipInstance.Launch();
        }
    }
}
