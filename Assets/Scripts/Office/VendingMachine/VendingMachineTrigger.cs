using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineTrigger : MonoBehaviour
{
    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GetComponentInParent<CanvasManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(PlayerConstants.action))
            {
                canvasManager.ActivateCanvas();
            }
            else if (Input.GetKeyDown(PlayerConstants.exit))
            {
                canvasManager.DeactivateCanvas();
            }
        }
    }
}
