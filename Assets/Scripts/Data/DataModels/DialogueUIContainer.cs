using UnityEngine;

[CreateAssetMenu(fileName = "DialogueUIContainer", menuName = "ScriptableObjects/DialogueUIContainer", order = 1)]
public class DialogueUIContainer : ScriptableObject
{
    [field: SerializeField]
    public GameObject MainDialogueUIPrefab { get; private set; }

    [field: SerializeField]
    public GameObject RandoDialogueUIPrefab { get; private set; }
}