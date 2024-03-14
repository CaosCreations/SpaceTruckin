using Events;
using PixelCrushers.DialogueSystem;
using UnityEngine;

[RequireComponent(typeof(Usable))]
public class NPCActor : InteractableObject
{
    private Usable usable;
    private bool usableLocked;

    private void Awake()
    {
        usable = GetComponent<Usable>();
    }

    protected override void Start()
    {
        base.Start();
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    public void OnUse(Transform player)
    {
        Debug.Log($"{gameObject.name} is being used by {player}.");
    }

    // Toggle usable on/off after being used 
    public void OnConversationStart(Transform actor)
    {
        Debug.Log($"{gameObject.name}'s conversation with {actor} is starting.");
        SetUsable(false);
    }

    // This is the Dialogue system actor one 
    public void OnConversationEnd(Transform actor)
    {
        Debug.Log($"{gameObject.name}'s conversation with {actor} is ending.");
        SetUsable(true);
    }

    // This is the singleton one 
    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        SetUsable(true);
    }

    public void SetUsable(bool value)
    {
        usable.enabled = value;
        usableLocked = !value;
    }

    // Toggle usable on/off based on whether player is in range/facing the right direction
    protected override void Update()
    {
        base.Update();

        if (usableLocked)
            return;

        usable.enabled = IsPlayerInteractable;
    }
}
