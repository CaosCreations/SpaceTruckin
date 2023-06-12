using Events;
using System;
using UnityEngine;

public class DialoguePositionConstrainer : MonoBehaviour
{
    [SerializeField]
    private DialoguePositionConstrainerSettings settings;

    private bool active;

    private void Start()
    {
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;

        DialogueUtils.StartConversationById(settings.PlayId);

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
}