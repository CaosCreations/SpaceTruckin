using PixelCrushers.DialogueSystem;
using UnityEngine;

[RequireComponent(typeof(Usable))]
public class NPCActor : MonoBehaviour
{
    private Usable usable;
    
    private void Awake()
    {
        usable = GetComponent<Usable>();
    }

    public void OnUse(Transform player)
    {
        Debug.Log($"{gameObject.name} is being used by {player}.");
        SetUsableContentVisible(false);
    }

    public void OnConversationEnd(Transform actor)
    {
        Debug.Log($"{gameObject.name}'s conversation with {actor} is ending.");
        SetUsableContentVisible(true);
    }

    private void SetUsableContentVisible(bool visible)
    {
        if (visible)
        {
            // Remove overrides if visible 
            usable.overrideName = string.Empty;
            usable.overrideUseMessage = string.Empty;
        }
        else
        {
            usable.overrideName = " ";
            usable.overrideUseMessage = " ";
        }
    }
}