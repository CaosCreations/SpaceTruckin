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

    // Toggle usable on/off after being used 
    public void OnUse(Transform player)
    {
        Debug.Log($"{gameObject.name} is being used by {player}.");
        usable.enabled = false;
        usableLocked = true;
    }

    public void OnConversationStart(Transform actor)
    {
        Debug.Log($"{gameObject.name}'s conversation with {actor} is starting.");
        usable.enabled = false;
        usableLocked = true;
    }

    public void OnConversationEnd(Transform actor)
    {
        Debug.Log($"{gameObject.name}'s conversation with {actor} is ending.");
        usable.enabled = true;
        usableLocked = false;
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
