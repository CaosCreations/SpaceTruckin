using System;
using TMPro;
using UnityEngine;

public enum UICanvasType
{
    None, Terminal, Vending, Hangar, Cassette, NoticeBoard, MainMenu, Bed
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UICanvasBase bedCanvas;
    public UICanvasBase terminalCanvas;
    public UICanvasBase vendingCanvas;
    public UICanvasBase hangarNodeCanvas;
    public UICanvasBase casetteCanvas;
    public UICanvasBase noticeBoardCanvas;
    public UICanvasBase mainMenuCanvas;

    public bool currentMenuOverridesEscape;
    public TextMeshPro interactionTextMesh;

    public static UICanvasType currentCanvasType;
    public static int hangarNode;

    public static event Action OnCanvasActivated;
    public static event Action OnCanvasDeactivated;

    private void Awake()
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
        Init();
    }

    public void Init()
    {
        if (DataUtils.IsNewGame())
        {
            // Show the main menu canvas for character creation
            currentCanvasType = UICanvasType.MainMenu;
            ShowCanvas();
        }
    }

    private void Update()
    {
        if (!PlayerManager.IsPaused 
            && currentCanvasType != UICanvasType.None 
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            ShowCanvas();
        }
        else if (Input.GetKeyDown(PlayerConstants.ExitKey) && !currentMenuOverridesEscape)
        {
            ClearCanvases();
        }

        if (currentCanvasType != UICanvasType.None)
        {
            interactionTextMesh.gameObject.SetActive(true);
            interactionTextMesh.SetText(GetInteractionString());
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
        PlayerManager.IsPaused = false;
        Instance.bedCanvas.SetActive(false);
        Instance.terminalCanvas.SetActive(false);
        Instance.hangarNodeCanvas.SetActive(false);
        Instance.vendingCanvas.SetActive(false);
        Instance.casetteCanvas.SetActive(false);
        Instance.noticeBoardCanvas.SetActive(false);
        Instance.mainMenuCanvas.SetActive(false);
    }

    public static void ShowCanvas()
    {
        ClearCanvases();
        PlayerManager.Instance.EnterMenuState();
        UICanvasBase canvas = GetCanvasByType(currentCanvasType);
        canvas.SetActive(true);

        // Show tutorial overlay if first time using the UI 
        if (!CurrentCanvasHasBeenViewed())
        {
            canvas.ShowTutorial();
        }

        // Main menu is not proximity-based, so we reset the current type
        if (currentCanvasType == UICanvasType.MainMenu)
        {
            currentCanvasType = UICanvasType.None;
        }
    }

    private static UICanvasBase GetCanvasByType(UICanvasType canvasType)
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
            case UICanvasType.MainMenu:
                return Instance.mainMenuCanvas;
            default:
                Debug.LogError("Invalid UI type passed to GetCanvasByType");
                return null;
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
            case UICanvasType.MainMenu:
            default:
                return string.Empty;
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
