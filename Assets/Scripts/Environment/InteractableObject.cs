using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Set this OnEnter and OnExit to avoiding repeatedly comparing tags
    public bool IsPlayerColliding;

    private bool PlayerIsColliding(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerIsColliding(other))
        {
            IsPlayerColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerIsColliding(other))
        {
            IsPlayerColliding = false;
        }
    }
}
