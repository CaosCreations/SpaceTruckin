using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSoundEffectContainer", menuName = "ScriptableObjects/Dialogue/DialogueSoundEffectContainer", order = 1)]
public class DialogueSoundEffectContainer : ScriptableObject, IScriptableObjectContainer<DialogueSoundEffect>
{
    [field: SerializeField]
    public DialogueSoundEffect[] Elements { get; set; }
}
