﻿using UnityEngine;

public class HangarNodeUIActivator : MonoBehaviour
{
    public int hangarNode;

    private void OnTriggerEnter(Collider other)
    {
        Ship shipForNode = HangarManager.GetShipByNode(hangarNode);
        if(shipForNode != null && !shipForNode.IsLaunched)
        {
            UIManager.SetCanInteract(UICanvasType.Hangar, hangarNode);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.SetCannotInteract();
    }

    private void OnValidate()
    {
        if (!HangarManager.NodeIsValid(hangarNode))
        {
            Debug.Log("Invalid node number entered in inspector");
            hangarNode = 1; 
        }
    }
}
