using UnityEngine;

public class UIActivator : InteractableObject
{
    public UICanvasType canvasType;

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerInteractable)
        {
            UIManager.SetCanInteract(canvasType);
        }
        else if (UIManager.CurrentCanvasType == canvasType)
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
