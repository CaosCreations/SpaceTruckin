using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UICanvasType
{
    Bed, Terminal, Vending, Hangar, Cassette, NoticeBoard, None
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject bedCanvas;
    public GameObject terminalCanvas;
    public GameObject vendingCanvas;
    public GameObject hangarNodeCanvas;
    public GameObject casetteCanvas;
    public GameObject noticeBoardCanvas;

    public UICanvasType interactableType;

    public static event Action onCanvasActivated;
    public static event Action onCanvasDeactivated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ClearCanvases();
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.action) && interactableType != UICanvasType.None)
        {
            ShowCanvas();
        }
        if (Input.GetKeyDown(PlayerConstants.exit))
        {
            Time.timeScale = 1;
            ClearCanvases();
        }
    }

    public static void ClearCanvases()
    {
        Instance.bedCanvas.SetActive(false);
        Instance.terminalCanvas.SetActive(false);
        Instance.hangarNodeCanvas.SetActive(false);
        Instance.vendingCanvas.SetActive(false);
        Instance.casetteCanvas.SetActive(false);
        Instance.noticeBoardCanvas.SetActive(false);
    }

    public static void ShowCanvas()
    {
        ClearCanvases();
        Time.timeScale = 0;

        switch (Instance.interactableType)
        {
            case UICanvasType.Terminal:
                Instance.terminalCanvas.SetActive(true);
                break;
            case UICanvasType.Hangar:
                Instance.hangarNodeCanvas.SetActive(true);
                break;
            case UICanvasType.Vending:
                Instance.vendingCanvas.SetActive(true);
                break;
            case UICanvasType.Cassette:
                Instance.casetteCanvas.SetActive(true);
                break;
            case UICanvasType.NoticeBoard:
                Instance.noticeBoardCanvas.SetActive(true);
                break;
            case UICanvasType.Bed:
                Instance.bedCanvas.SetActive(true);
                break;
        }
    }

    public static void SetCanInteract(UICanvasType type, bool canInteract)
    {
        if (canInteract)
        {
            Instance.interactableType = type;
        }
        else
        {
            Instance.interactableType = UICanvasType.None;
        }
    }
}
