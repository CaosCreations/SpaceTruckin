using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSoundEffect ", menuName = "ScriptableObjects/Dialogue/DialogueSoundEffect ", order = 1)]
public class DialogueSoundEffect : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public AudioClip Clip { get; private set; }
}
