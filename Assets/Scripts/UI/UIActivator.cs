using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivator : MonoBehaviour
{
    public UICanvasType type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == PlayerConstants.objectTag)
        {
            UIManager.SetCanInteract(type, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == PlayerConstants.objectTag)
        {
            UIManager.SetCanInteract(type, false);
        }
    }
}
