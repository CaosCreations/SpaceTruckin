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

    [SerializeField] private UICanvasBase bedCanvas;
    [SerializeField] private UICanvasBase terminalCanvas;
    [SerializeField] private UICanvasBase vendingCanvas;
    [SerializeField] private UICanvasBase hangarNodeCanvas;
    [SerializeField] private UICanvasBase casetteCanvas;
    [SerializeField] private UICanvasBase noticeBoardCanvas;
    [SerializeField] private UICanvasBase mainMenuCanvas;

    public bool CurrentMenuOverridesEscape;
    private TextMeshPro interactionTextMesh;
    private static UICanvasType currentCanvasType;
    
    public static int HangarNode;

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
            SetCanInteract(UICanvasType.MainMenu);
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
        else if (Input.GetKeyDown(PlayerConstants.ExitKey) && !CurrentMenuOverridesEscape)
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

    public static void SetCanInteract(UICanvasType canvasType, int node = -1)
    {
        currentCanvasType = canvasType;

        // We pass in a node value when opening the hangar UI
        if (canvasType == UICanvasType.Hangar && HangarManager.NodeIsValid(node))
        {
            HangarNode = node;
        }
    }

    public static void SetCannotInteract()
    {
        currentCanvasType = UICanvasType.None;
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
