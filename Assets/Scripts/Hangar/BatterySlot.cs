using UnityEngine;

public class BatterySlot : MonoBehaviour
{
    [SerializeField] private HangarSlot hangarSlot;
    private int nodeNumber;

    private void Start()
    {
        nodeNumber = hangarSlot.Node;
    }

    public void TransferEnergy(Battery battery)
    {
        if (hangarSlot != null && hangarSlot.Ship != null)
        {
            ShipsManager.EnableWarp(hangarSlot.Ship);
            battery.Discharge();
        }
    }

    private GameObject GetBattery(GameObject parent)
    {
        foreach (Transform child in parent.transform) 
        {
            if (child.CompareTag(HangarConstants.BatteryTag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag) 
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            Battery battery = other.GetComponentInChildren<Battery>();
            if (battery != null && battery.IsCharged)
            {
                TransferEnergy(battery);
            }
        }
    }

    private void Update()
    {

    }
}
