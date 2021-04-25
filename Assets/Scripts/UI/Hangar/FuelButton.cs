using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button Button { get; private set; }
    public bool IsFueling { get; private set; }

    private void Awake()
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
