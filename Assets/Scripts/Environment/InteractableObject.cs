using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Set this OnEnter and OnExit to avoiding repeatedly comparing tags
    public bool IsPlayerColliding { get; protected set; }

    protected bool PlayerIsColliding(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            return true;
        }
        return false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (PlayerIsColliding(other))
        {
            IsPlayerColliding = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (PlayerIsColliding(other))
        {
            IsPlayerColliding = false;
        }
    }
}
