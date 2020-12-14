﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UICanvasType
{
    Terminal, Vending, Hangar, Cassette, NoticeBoard, None
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject terminalCanvas;
    public GameObject vendingCanvas;
    public GameObject hangarNodeCanvas;
    public GameObject casetteCanvas;

    public UICanvasType interactableType;

    public static event Action onCanvasActivated;
    public static event Action onCanvasDeactivated;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (UIManager.Instance == null)
        {
            UIManager.Instance = this;
        }
        else if (UIManager.Instance == this)
        {
            Destroy(UIManager.Instance.gameObject);
            UIManager.Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
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
        Instance.terminalCanvas.SetActive(false);
        Instance.hangarNodeCanvas.SetActive(false);
        Instance.vendingCanvas.SetActive(false);
        Instance.casetteCanvas.SetActive(false);
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
