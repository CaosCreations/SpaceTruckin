using UnityEngine;

public class HangarNodeUIActivator : MonoBehaviour
{
    public HangarNode hangarNode;

    private void OnTriggerEnter(Collider other)
    {
        Ship shipForNode = ShipsManager.GetShipForNode(hangarNode);
        if(shipForNode != null && !shipForNode.IsLaunched)
        {
            UIManager.SetCanInteractHangarNode(hangarNode, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.SetCanInteractHangarNode(hangarNode, false);
    }
}
