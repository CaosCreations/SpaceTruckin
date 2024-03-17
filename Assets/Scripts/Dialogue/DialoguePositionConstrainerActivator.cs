using Events;
using System;
using UnityEngine;

public enum ConstrainerType
{
    Conversation, Popup,
}

public class DialoguePositionConstrainerActivator : MonoBehaviour
{
    [SerializeField] private ConstrainerType type = ConstrainerType.Conversation;

    [field: SerializeField]
    public string ZoneName { get; private set; }

    [SerializeField]
    private DialoguePositionConstrainerSettings settings;

    [SerializeField]
    private PopupConstrainerSettings popupSettings;

    [SerializeField] private bool deactivatesOnShipLaunched;

    private bool IsActive { get => constrainer.gameObject.activeSelf; set => constrainer.SetActive(value); }

    [SerializeField]
    private PositionConstrainer constrainer;

    private void Awake()
    {
        if (type == ConstrainerType.Conversation && settings.ActivateId <= 0)
        {
            Debug.LogError("Missing configuration for DialoguePositionConstrainerSettings");
            return;
        }

        if (type == ConstrainerType.Popup && string.IsNullOrWhiteSpace(popupSettings.Text))
        {
            Debug.LogError("Missing configuration for PopupConstrainerSettings");
            return;
        }

        constrainer.Init(OnConstrained);
        IsActive = false;
    }

    private void OnConstrained()
    {
        if (type == ConstrainerType.Conversation)
        {
            DialogueUtils.StartConversationById(settings.PlayId);
        }
        else if (type == ConstrainerType.Popup)
        {
            PopupManager.ShowPopup(type: popupSettings.PopupType, bodyText: popupSettings.Text);
        }
    }

    private void Start()
    {
        if (type == ConstrainerType.Conversation)
        {
            SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
        }
        else if (type == ConstrainerType.Popup)
        {
            SingletonManager.EventService.Add<OnShipLaunchedEvent>(OnShipLaunchedHandler);
        }
        else
        {
            Debug.LogError("Invalid constrainer type: " + type);
        }
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        if (type != ConstrainerType.Conversation)
        {
            return;
        }

        if (!IsActive && evt.Conversation.id == settings.ActivateId)
        {
            Activate();
            return;
        }

        if (IsActive && evt.Conversation.id == settings.DeactivateId)
        {
            Deactivate();
        }
    }

    private void OnShipLaunchedHandler()
    {
        if (type != ConstrainerType.Conversation && deactivatesOnShipLaunched)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        Debug.Log("DialoguePositionConstrainer activating...");
        IsActive = true;
    }

    public void Deactivate()
    {
        Debug.Log("DialoguePositionConstrainer deactivating...");
        IsActive = false;
    }
}

/// <summary>
/// Should really have a base class for the activator and then popup/dialogue as derived classes,
/// but for now just stick them both here.
/// </summary>
[Serializable]
public class PopupConstrainerSettings
{
    [field: SerializeField]
    public PopupType PopupType = PopupType.Default;

    [field: SerializeField]
    public string Text;
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