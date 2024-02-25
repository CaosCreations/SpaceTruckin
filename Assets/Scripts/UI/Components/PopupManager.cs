using UnityEngine;
using UnityEngine.Events;

public enum PopupType
{
    Default, DemoFeature,
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private Canvas canvas;
    [SerializeField] private Popup defaultPopup;
    [SerializeField] private Popup demoFeaturePopup;
    private Popup currentPopup;
    public static bool IsPopupActive => Instance.currentPopup != null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ShowPopup(UnityAction onOk = null, UnityAction onCancel = null, string bodyText = null, string okText = null, string cancelText = null, PopupType type = PopupType.Default)
    {
        UIManager.AddOverriddenKey(KeyCode.Escape);
        PlayerManager.EnterPausedState();
        Instance.canvas.gameObject.SetActive(true);

        Instance.currentPopup = type == PopupType.Default ? Instance.defaultPopup : Instance.demoFeaturePopup;
        Debug.Log("Showing popup: " + Instance.currentPopup.name);
        Instance.currentPopup.SetActive(true);
        Instance.currentPopup.Init(() =>
        {
            Instance.OnHide();
            onOk?.Invoke();
        }, bodyText, okText, cancelText, () =>
        {
            Instance.OnHide();
            onCancel?.Invoke();
        }, onCancel != null);
    }

    private void OnHide()
    {
        if (!UIManager.IsCanvasActive(false))
        {
            UIManager.RemoveOverriddenKey(KeyCode.Escape);
            PlayerManager.ExitPausedState();
        }
        canvas.gameObject.SetActive(false);
        defaultPopup.SetActive(false);
        demoFeaturePopup.SetActive(false);
        currentPopup = null;
    }

    private void Update()
    {
        if (canvas.gameObject.activeSelf && Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            //HidePopup();
        }
    }
}
