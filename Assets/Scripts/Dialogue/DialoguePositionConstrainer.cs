using Events;
using System;
using UnityEngine;

public class DialoguePositionConstrainer : MonoBehaviour
{
    [SerializeField]
    private DialoguePositionConstrainerSettings settings;

    private BoxCollider boxCollider;
    private bool active;

    [SerializeField]
    private float resetDistance = 1f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active || !other.CompareTag(PlayerConstants.PlayerTag))
            return;

        DialogueUtils.StartConversationById(settings.PlayId);

        Vector3 centralPoint = boxCollider.bounds.GetCentralPointAlongLongEdge();
        Vector3 playerPosition = other.transform.position;

        Vector3 offset = playerPosition - centralPoint;
        Vector3 newPosition;

        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.z))
        {
            // Player entered from the X-aligned long edge
            float newX = centralPoint.x + Mathf.Sign(offset.x) * boxCollider.bounds.extents.x;
            newPosition = new Vector3(newX, playerPosition.y, playerPosition.z);
        }
        else
        {
            // Player entered from the Z-aligned long edge
            float newZ = centralPoint.z + Mathf.Sign(offset.z) * boxCollider.bounds.extents.z;
            newPosition = new Vector3(playerPosition.x, playerPosition.y, newZ);
        }

        Vector3 oppositeDirection = (playerPosition - newPosition).normalized;
        newPosition += oppositeDirection * resetDistance;

        other.transform.position = newPosition;
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        if (!active && evt.Conversation.id == settings.ActivateId)
        {
            Debug.Log("DialoguePositionConstrainer activating...");
            active = true;
            return;
        }

        if (active && evt.Conversation.id == settings.DeactivateId)
        {
            Debug.Log("DialoguePositionConstrainer deactivating...");
            active = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!active)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size);
    }
}

[Serializable]
public class DialoguePositionConstrainerSettings
{
    /// <summary>
    /// Which convo enables the constrainer
    /// </summary>
    [field: SerializeField]
    public int ActivateId { get; private set; }

    /// <summary>
    /// Which convo disables the constrainer
    /// </summary>
    [field: SerializeField]
    public int DeactivateId { get; private set; }

    /// <summary>
    /// Which convo is played on entering constrainer trigger
    /// </summary>
    [field: SerializeField]
    public int PlayId { get; private set; }
}