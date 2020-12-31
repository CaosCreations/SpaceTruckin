using UnityEngine;
using UnityEngine.UI;

public class MessageButtonHandler : MonoBehaviour
{
    public Button button;
    public Text text;

    public void SetMessage(Message message)
    {
        text.text = $"<b>{message.sender}</b>: {message.subject}";
    }
}