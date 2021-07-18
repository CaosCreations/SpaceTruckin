using UnityEngine;

// Simple handler for office door box collider
public class OfficeDoor : InteractableObject
{
    [SerializeField] private Vector3 doorOpenOffset;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (IsPlayerColliding)
        {
            OpenDoor();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        transform.parent.Translate(doorOpenOffset);
        transform.Translate(-doorOpenOffset);

        IsPlayerColliding = true;
    }

    public void CloseDoor()
    {
        transform.parent.Translate(-doorOpenOffset);
        transform.Translate(doorOpenOffset);

        IsPlayerColliding = false;
    }
}
