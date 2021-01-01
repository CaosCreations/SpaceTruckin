using UnityEngine;
using UnityEngine.UI;

public class MessageButtonHandler : MonoBehaviour
{
    //public Button button;
    //public Text text;

    private void Start()
    {
        
    }

    public void Init(Message message, UnityEngine.Events.UnityAction callback)
    {
        GetComponent<Button>().AddOnClick(callback);
        GetComponentInChildren<Text>().text = $"<b>{message.sender}</b>: {message.subject}";

        //button = GetComponent<Button>();
        //button.AddOnClick(callback);
        //text = GetComponentInChildren<Text>();
        //text.text = $"<b>{message.sender}</b>: {message.subject}";

    }

    public void SetMessage(Message message)
    {
        //text.text = $"<b>{message.sender}</b>: {message.subject}";
        //GetComponentInChildren<Text>().text = $"<b>{message.sender}</b>: {message.subject}";

    }
}