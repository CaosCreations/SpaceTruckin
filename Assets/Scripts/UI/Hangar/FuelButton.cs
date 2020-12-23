using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public bool isFueling = false;

    private void Awake()
    {
        button = GetComponent<Button>();   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isFueling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isFueling = false;
    }
}
