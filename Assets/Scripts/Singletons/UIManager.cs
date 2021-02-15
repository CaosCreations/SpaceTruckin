using System;
using TMPro;
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
    public bool currentMenuOverridesEscape;
    public TextMeshPro interactionTextMesh;

    public UICanvasType interactableType;
    public HangarNode hangarNode;

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
        interactionTextMesh = GetComponentInChildren<TextMeshPro>();
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
        if (Input.GetKeyDown(PlayerConstants.exit) && !currentMenuOverridesEscape)
        {
            ClearCanvases();
        }
        
        if(interactableType != UICanvasType.None)
        {
            interactionTextMesh.gameObject.SetActive(true);
            interactionTextMesh.text = GetInteractionString();
            interactionTextMesh.transform.position = PlayerManager.Instance.playerMovement.transform.position + new Vector3(0, 0.5f, 0);
        }
        else
        {
            interactionTextMesh.gameObject.SetActive(false);
        }
    }

    public static void ClearCanvases()
    {
        PlayerManager.Instance.isPaused = false;
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
        PlayerManager.Instance.EnterMenuState();

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

    public static void SetCanInteractHangarNode(HangarNode node, bool canInteract)
    {
        if (canInteract)
        {
            Instance.interactableType = UICanvasType.Hangar;
            Instance.hangarNode = node;
        }
        else
        {
            Instance.interactableType = UICanvasType.None;
        }
    }

    private static string GetInteractionString()
    {
        string interaction = "Press E to ";
        switch (Instance.interactableType)
        {
            case UICanvasType.Bed:
                interaction += "Sleep";
                break;
            case UICanvasType.Cassette:
                interaction += "Play Music";
                break;
            case UICanvasType.Hangar:
                interaction += "Manage Ship";
                break;
            case UICanvasType.NoticeBoard:
                interaction += "Accept Missions";
                break;
            case UICanvasType.Terminal:
                interaction += "Manage Company";
                break;
            case UICanvasType.Vending:
                interaction += "Buy Snax";
                break;
        }

        return interaction;
    }
}
