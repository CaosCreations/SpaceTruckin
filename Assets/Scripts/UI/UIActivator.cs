using UnityEngine;

public class UIActivator : InteractableObject
{
    public UICanvasType canvasType;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag) && IsPlayerInteractable)
        {
            UIManager.SetCanInteract(canvasType);
        }
        else
        {
            UIManager.SetCannotInteract();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            UIManager.SetCannotInteract();
        }
    }
}
