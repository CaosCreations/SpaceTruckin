using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Set this OnEnter and OnExit to avoiding repeatedly comparing tags
    public bool IsPlayerColliding { get; protected set; }

    public Collider Collider;
    public GameObject RaycastTarget;

    [SerializeField]
    private SpriteRenderer interactableIcon;

    private void Start()
    {
        // Default raycast target to self 
        if (RaycastTarget == null)
            RaycastTarget = gameObject;

        if (interactableIcon != null)
            interactableIcon.gameObject.SetActive(false);
    }

    /// <summary>
    /// Is the player in range and facing the direction of the interactable object.
    /// </summary>
    public bool IsPlayerInteractable => IsPlayerColliding && PlayerManager.IsFirstRaycastHit(RaycastTarget);

    protected virtual bool IsIconVisible => IsPlayerInteractable;

    /// <summary>
    /// Can be called whenever we want to check whether the object is colliding with the player
    /// As it doesn't rely on trigger OnTriggerEnter and OnTriggerExit, it can be used in the rare situations where the object's
    /// position is changed without triggering OnTriggerExit (when being dropped from the player's head for instance)
    /// </summary>
    protected void SetPlayerIsColliding()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Collider.bounds.extents, Quaternion.identity);

        foreach (Collider item in colliders)
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            IsPlayerColliding = true;
        }

        Debug.Log("IsPlayerColliding = " + IsPlayerColliding);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("On trigger exit Interactable. " + other.name);

        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            IsPlayerColliding = false;
        }

        Debug.Log("IsPlayerColliding = " + IsPlayerColliding);
    }

    protected virtual void Update()
    {
        if (interactableIcon == null)
            return;

        interactableIcon.gameObject.SetActive(IsIconVisible);
    }
}
