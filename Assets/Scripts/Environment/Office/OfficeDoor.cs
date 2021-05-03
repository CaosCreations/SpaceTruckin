using UnityEngine;

// Simple handler for office door box collider
public class OfficeDoor : InteractableObject
{
    [SerializeField] private Vector3 doorOpenOffset;

    private void OnTriggerEnter(Collider other)
    {
        OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        CloseDoor();
    }

    private void OpenDoor()
    {
        transform.parent.Translate(doorOpenOffset);
        transform.Translate(-doorOpenOffset);
    }

    public void CloseDoor()
    {
        transform.parent.Translate(-doorOpenOffset);
        transform.Translate(doorOpenOffset);
    }
}
