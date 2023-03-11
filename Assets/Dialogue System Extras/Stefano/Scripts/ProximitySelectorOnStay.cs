using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

/// <summary>
/// Same as the base <see cref="ProximitySelector"/> but does additional check in <see cref="OnTriggerStay(Collider)"/>
/// </summary>
public class ProximitySelectorOnStay : ProximitySelector
{
    protected void OnTriggerStay(Collider other)
    {
        CheckTriggerEnter(other.gameObject);
    }
}