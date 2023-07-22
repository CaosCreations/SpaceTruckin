using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject popup;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private string defaultOkText = "OK";
    [SerializeField] private string defaultCancelText = "Cancel";
    [SerializeField] private Text bodyText;

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

    public static void ShowPopup(UnityAction onOk = null, UnityAction onCancel = null, string bodyText = null, string okText = null, string cancelText = null)
    {
        UIManager.AddOverriddenKey(KeyCode.Escape);
        PlayerManager.EnterPausedState();
        Instance.canvas.gameObject.SetActive(true);

        Instance.okButton.AddOnClick(() =>
        {
            onOk?.Invoke();
            Instance.HidePopup();
        });

        // Some popups only have ok button 
        if (onCancel != null)
        {
            Instance.cancelButton.gameObject.SetActive(true);
            Instance.cancelButton.AddOnClick(() =>
            {
                onCancel();
                Instance.HidePopup();
            });
        }
        Instance.SetButtonTexts(bodyText, okText, cancelText);
    }

    private void HidePopup()
    {
        UIManager.RemoveOverriddenKey(KeyCode.Escape);
        PlayerManager.ExitPausedState();
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    private void SetButtonTexts(string bodyText, string okText = null, string cancelText = null)
    {
        this.bodyText.SetText(bodyText);
        okButton.SetText(okText ?? defaultOkText);
        cancelButton.SetText(cancelText ?? defaultCancelText);
    }

    private void Update()
    {
        if (canvas.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            //HidePopup();
        }
    }
}
