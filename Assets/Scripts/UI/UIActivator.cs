using UnityEngine;

public class UIActivator : MonoBehaviour
{
    public UICanvasType canvasType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            UIManager.SetCanInteract(canvasType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            UIManager.SetCannotInteract();
        }
    }
}
