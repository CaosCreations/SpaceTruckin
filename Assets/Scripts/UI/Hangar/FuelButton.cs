using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FuelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button Button { get; private set; }
    public bool IsFueling { get; private set; }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        Button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsFueling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsFueling = false;
    }
}
