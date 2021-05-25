using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Set this OnEnter and OnExit to avoiding repeatedly comparing tags
    public bool IsPlayerColliding { get; protected set; }

    protected void SetPlayerIsCollidingFromPlayerCollider(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            IsPlayerColliding = true;
        }

        else
        {
            IsPlayerColliding = false;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        SetPlayerIsCollidingFromPlayerCollider(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        SetPlayerIsCollidingFromPlayerCollider(other);
    }

    // Can be called whenever we want to check whether the object is colliding with the player
    // As it doesn't rely on trigger OnTriggerEnter and OnTriggerExit, it can be used in the rare situations where the object's
    // position is changed without triggering OnTriggerExit (when being dropped from the player's head for instance)
    protected void SetPlayerIsCollidingFromInteractableObjectCollider()
    {
        Collider collider = GetComponent<Collider>();

        Collider[] colliders = Physics.OverlapBox(transform.position, collider.bounds.extents, Quaternion.identity);

        Debug.Log("SetPlayerIsCollidingFromInteractableObjectCollider(). Colliding with " + colliders.Length + "colliders :");

        foreach(Collider item in colliders)
        {
            Debug.Log(item.name);   

            if (item.CompareTag(PlayerConstants.PlayerTag))
            {
                IsPlayerColliding = true;
                return;
            }
        }

        IsPlayerColliding = false;
    }
}
