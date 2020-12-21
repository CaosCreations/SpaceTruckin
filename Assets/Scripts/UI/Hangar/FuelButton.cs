using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isFueling = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isFueling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isFueling = false;
    }
}
