using Events;
using System;
using UnityEngine;

public class DialoguePositionConstrainerActivator : MonoBehaviour
{
    [SerializeField]
    private DialoguePositionConstrainerSettings settings;

    private bool IsActive { get => constrainer.gameObject.activeSelf; set => constrainer.SetActive(value); }

    [SerializeField]
    private PositionConstrainer constrainer;

    private void Awake()
    {
        constrainer.Init(() => DialogueUtils.StartConversationById(settings.PlayId));
        IsActive = false;
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        if (!IsActive && evt.Conversation.id == settings.ActivateId)
        {
            Debug.Log("DialoguePositionConstrainer activating...");
            IsActive = true;
            return;
        }

        if (IsActive && evt.Conversation.id == settings.DeactivateId)
        {
            Debug.Log("DialoguePositionConstrainer deactivating...");
            IsActive = false;
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