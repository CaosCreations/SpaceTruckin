using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas canvas; 

    public static event Action onCanvasActivated;
    public static event Action onCanvasDeactivated;

    public void ActivateCanvas()
    {
        //Time.timeScale = 0;
        canvas.gameObject.SetActive(true);
        onCanvasActivated?.Invoke();
    }

    public void DeactivateCanvas()
    {
        //Time.timeScale = 1;
        canvas.gameObject.SetActive(false);
        onCanvasDeactivated?.Invoke();
    }

    public void ToggleCanvas()
    {
        canvas.gameObject.SetActive(!gameObject.activeSelf);
    }
}
