using PixelCrushers.DialogueSystem;
using System.Linq;
using UnityEngine;

public enum InteractionSoundEffect
{
    DoorOpening, DoorClosing, DialogueMessageReceived
}

public class InteractionSoundEffectsManager : SoundEffectsManager<InteractionSoundEffectsManager, InteractionSoundEffect>, ILuaFunctionRegistrar
{
    [SerializeField] private AudioClip doorOpeningClip;
    [SerializeField] private AudioClip doorClosingClip;
    // Static dialogue clip 
    [SerializeField] private AudioClip dialogueMessageReceivedClip;
    // Dynamic dialogue clips 
    [SerializeField] private DialogueSoundEffectContainer dialogueSoundEffectContainer;
    [SerializeField] private AudioSource dialogueSoundEffectSource;

    public override AudioClip GetClipBySoundEffectType(InteractionSoundEffect effectType)
    {
        return effectType switch
        {
            InteractionSoundEffect.DoorOpening => doorOpeningClip,
            InteractionSoundEffect.DoorClosing => doorClosingClip,
            InteractionSoundEffect.DialogueMessageReceived => dialogueMessageReceivedClip,
            _ => default,
        };
    }

    private void PlayDialogueSoundEffect(string name)
    {
        var soundEffect = dialogueSoundEffectContainer.Elements.FirstOrDefault(e => e.Name == name);
        if (soundEffect == null)
        {
            Debug.Log($"No dialogue sound effect with name \"{name}\".");
            return;
        }
        PlayAudioClip(soundEffect.Clip, dialogueSoundEffectSource);
    }

    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(
            DialogueConstants.PlaySoundEffectFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => PlayDialogueSoundEffect(string.Empty)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.PlaySoundEffectFunctionName);
    }
}
