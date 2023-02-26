using Events;
using UnityEngine;

public class HangarNodeUIActivator : InteractableObject
{
    public int hangarNode;

    protected override void Start()
    {
        base.Start();
        SingletonManager.EventService.Add<OnShipLaunchedEvent>(OnShipLaunched);
    }

    protected override bool IsIconVisible => IsPlayerInteractable && UIManager.HangarNode == hangarNode;

    private void OnShipLaunched()
    {
        // Hide icon when ship is launched 
        interactableIcon.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        Ship shipForNode = HangarManager.GetShipByNode(hangarNode);

        // Corresponding ship must be at this node and the player in an interactable state
        if (other.CompareTag(PlayerConstants.PlayerTag)
            && shipForNode != null
            && !shipForNode.IsLaunched
            && IsPlayerInteractable)
        {
            UIManager.SetCanInteract(UICanvasType.Hangar, hangarNode);
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

    private void OnValidate()
    {
        if (!HangarManager.NodeIsValid(hangarNode))
        {
            Debug.Log("Invalid node number entered in inspector");
            hangarNode = 1;
        }
    }
}
