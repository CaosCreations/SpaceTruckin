using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveCanvasTutorialCard : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseButtonHandler);
    }

    public void AddCloseButtonListener(Action action)
    {
        closeButton.onClick.AddListener(() => action());
    }

    private void CloseButtonHandler()
    {
        gameObject.SetActive(false);
    }
}