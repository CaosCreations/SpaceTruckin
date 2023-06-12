using Events;
using System;
using UnityEngine;

public class DialoguePositionConstrainer : MonoBehaviour
{
    [SerializeField]
    private DialoguePositionConstrainerSettings settings;

    private Vector3 playerResetPos;

    private bool active;

    private void Awake()
    {
        var colliderBounds = GetComponent<BoxCollider>().bounds;
        playerResetPos = GetPlayerResetPos(colliderBounds);
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    private Vector3 GetPlayerResetPos(Bounds bounds)
    {
        float centerX = bounds.center.x;
        float centerZ = bounds.center.z;
        float lengthX = bounds.size.x;
        float lengthZ = bounds.size.z;

        // If the rectangle is aligned with the X-axis
        Vector3 centralPointX = new Vector3(centerX + lengthX / 2f, bounds.center.y, bounds.center.z);

        // If the rectangle is aligned with the Z-axis
        Vector3 centralPointZ = new Vector3(bounds.center.x, bounds.center.y, centerZ + lengthZ / 2f);
        return centralPointZ + new Vector3(0f, 0f, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;

        DialogueUtils.StartConversationById(settings.PlayId);
        PlayerManager.PlayerMovement.SetPosition(playerResetPos);

        // TODO: Turn player around 
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

    [field: SerializeField]
    public
}