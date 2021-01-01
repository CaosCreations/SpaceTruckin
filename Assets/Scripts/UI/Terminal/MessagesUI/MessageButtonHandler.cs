using UnityEngine;
using UnityEngine.UI;

public class MessageButtonHandler : MonoBehaviour
{
    public void Init(Message message, UnityEngine.Events.UnityAction callback)
    {
        GetComponent<Button>().AddOnClick(callback);
        GetComponentInChildren<Text>().text = $"<b>{message.sender}</b>: {message.subject}";
    }
}