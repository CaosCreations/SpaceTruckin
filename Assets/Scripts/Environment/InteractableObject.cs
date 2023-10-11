using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Collider Collider;
    public GameObject RaycastTarget;

    [SerializeField]
    protected SpriteRenderer interactableIcon;

    [SerializeField]
    private bool isPlayerColliding;
    [SerializeField]
    private bool isPlayerInteractable;
    private string layerName;

    public bool IsPlayerColliding
    {
        get { return isPlayerColliding; }
        protected set { isPlayerColliding = value; }
    }

    public bool IsPlayerInteractable
    {
        get
        {
            isPlayerInteractable = IsPlayerColliding && PlayerManager.IsPlayerFacingObject(RaycastTarget, layerName);
            return isPlayerInteractable;
        }
    }

    protected virtual bool IsIconVisible => !PlayerManager.IsPaused && IsPlayerInteractable;

    protected virtual void Start()
    {
        // Default raycast target to self 
        if (RaycastTarget == null)
            RaycastTarget = gameObject;

        if (interactableIcon != null)
            interactableIcon.gameObject.SetActive(false);

        layerName = LayerMask.LayerToName(gameObject.layer);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            IsPlayerColliding = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            IsPlayerColliding = false;
        }
    }

    protected virtual void Update()
    {
        if (interactableIcon == null)
            return;

        interactableIcon.gameObject.SetActive(IsIconVisible);
    }
}
