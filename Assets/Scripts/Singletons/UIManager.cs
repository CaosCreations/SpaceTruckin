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

    public static UICanvasType currentCanvasType;
    public static int hangarNode;

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
        if (Input.GetKeyDown(PlayerConstants.ActionKey) && currentCanvasType != UICanvasType.None)
        {
            ShowCanvas();
        }
        if (Input.GetKeyDown(PlayerConstants.ExitKey) && !currentMenuOverridesEscape)
        {
            ClearCanvases();
        }
        
        if (currentCanvasType != UICanvasType.None)
        {
            interactionTextMesh.gameObject.SetActive(true);
            interactionTextMesh.text = GetInteractionString();
            interactionTextMesh.transform.position = 
                PlayerManager.PlayerMovement.transform.position + new Vector3(0, 0.5f, 0);
        }
        else
        {
            interactionTextMesh.gameObject.SetActive(false);
        }
    }

    public static void ClearCanvases()
    {
        PlayerManager.Instance.IsPaused = false;
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
        GetCanvasByType(currentCanvasType).SetActive(true);
        
        // Show tutorial overlay if first time using the UI 
        if (!CurrentCanvasHasBeenViewed())
        {

        }
    }

    private static GameObject GetCanvasByType(UICanvasType canvasType)
    {
        switch (canvasType)
        {
            case UICanvasType.Terminal:
                return Instance.terminalCanvas;
            case UICanvasType.Hangar:
                return Instance.hangarNodeCanvas;
            case UICanvasType.Vending:
                return Instance.vendingCanvas;
            case UICanvasType.Cassette:
                return Instance.casetteCanvas;
            case UICanvasType.NoticeBoard:
                return Instance.noticeBoardCanvas;
            case UICanvasType.Bed:
                return Instance.bedCanvas;
            default:
                Debug.LogError("Invalid UI type passed to GetCanvasByType");
                return default;
        }
    }

    public static void SetCanInteract(UICanvasType canvasType, bool canInteract)
    {
        if (canInteract)
        {
            currentCanvasType = canvasType;
        }
        else
        {
            currentCanvasType = UICanvasType.None;
        }
    }

    public static void SetCanInteractHangarNode(int node, bool canInteract)
    {
        if (canInteract)
        {
            currentCanvasType = UICanvasType.Hangar;
            hangarNode = node;
        }
        else
        {
            currentCanvasType = UICanvasType.None;
        }
    }

    private static string GetInteractionString()
    {
        string interaction = "Press E to ";
        switch (currentCanvasType)
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

    private static bool CurrentCanvasHasBeenViewed()
    {
        return PlayerPrefsManager.GetHasBeenViewedPref(currentCanvasType);
    }

    public static void SetCurrentCanvasHasBeenViewed(bool value)
    {
        PlayerPrefsManager.SetHasBeenViewedPref(currentCanvasType, value);
    }
}
