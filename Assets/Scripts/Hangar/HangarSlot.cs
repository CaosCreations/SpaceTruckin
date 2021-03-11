using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    public int node;
    public ShipInstance shipInstance;

    public void LaunchShip()
    {
        if (shipInstance != null)
        {
            shipInstance.Launch();
        }
    }
}
