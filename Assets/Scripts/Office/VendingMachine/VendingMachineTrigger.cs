using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineTrigger : MonoBehaviour
{
    private VendingMachineManager vendingMachineManager;

    private void Start()
    {
        vendingMachineManager = GetComponentInParent<VendingMachineManager>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        UIManager.SetCanInteract(UICanvasType.Vending, true);
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.SetCanInteract(UICanvasType.Vending, false);
    }
}
