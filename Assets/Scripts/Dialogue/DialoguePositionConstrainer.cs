using Events;
using System;
using UnityEngine;

public class DialoguePositionConstrainer : MonoBehaviour
{
    [SerializeField]
    private DialoguePositionConstrainerSettings settings;

    private BoxCollider rectangleCollider;

    [SerializeField]
    private bool active;

    [SerializeField]
    private float offsetDistance = 1f;

    private void Awake()
    {
        rectangleCollider = GetComponent<BoxCollider>();
        active = false;
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

        Vector3 playerPosition = other.transform.position;
        Vector3 centralPoint = rectangleCollider.bounds.GetCentralPointAlongLongEdge();
        Vector3 newPosition = GetResetPosition(playerPosition, centralPoint);
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

    private Vector3 GetResetPosition(Vector3 playerPosition, Vector3 centralPoint)
    {
        float offsetZ = playerPosition.z - centralPoint.z;
        float offsetX = playerPosition.x - centralPoint.x;

        if (Mathf.Abs(offsetZ) > Mathf.Abs(offsetX))
        {
            if (offsetZ > 0)
            {
                return new Vector3(centralPoint.x, centralPoint.y, centralPoint.z - offsetDistance);
            }
            else
            {
                return new Vector3(centralPoint.x, centralPoint.y, centralPoint.z + offsetDistance);
            }
        }
        else
        {
            if (offsetX > 0)
            {
                return new Vector3(centralPoint.x - offsetDistance, centralPoint.y, centralPoint.z);
            }
            else
            {
                return new Vector3(centralPoint.x + offsetDistance, centralPoint.y, centralPoint.z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!active)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + rectangleCollider.center, rectangleCollider.size);
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