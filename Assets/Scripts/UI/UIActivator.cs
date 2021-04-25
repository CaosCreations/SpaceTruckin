using UnityEngine;

public class UIActivator : MonoBehaviour
{
    public UICanvasType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            UIManager.SetCanInteract(type, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            UIManager.SetCanInteract(type, false);
        }
    }
}
