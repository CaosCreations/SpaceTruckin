using UnityEngine;

[CreateAssetMenu(fileName = "MessageContainer", menuName = "ScriptableObjects/MessageContainer", order = 1)]
public class MessageContainer : ScriptableObject, IScriptableObjectContainer<Message>
{
    [field: SerializeField]
    public Message[] Elements { get; set; }
}
