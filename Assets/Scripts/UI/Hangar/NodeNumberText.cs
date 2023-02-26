using Events;
using UnityEngine;
using UnityEngine.UI;

public class NodeNumberText : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        SingletonManager.EventService.Add<OnHangarNodeTerminalOpenedEvent>(SetText);
    }

    private void SetText(OnHangarNodeTerminalOpenedEvent openedEvent)
    {
        text.SetText($"Node #{UIManager.HangarNode}", FontType.Title);
    }
}