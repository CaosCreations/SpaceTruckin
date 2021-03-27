using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Set this OnEnter and OnExit to avoiding repeatedly comparing tags
    public bool IsPlayerColliding { get; private set; }

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
            Debug.Log("Colliding true");
            IsPlayerColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerIsColliding(other))
        {
            Debug.Log("Colliding false");
            IsPlayerColliding = false;
        }
    }
}
