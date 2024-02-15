using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Text bodyText;

    [SerializeField] private string defaultOkText = "OK";
    [SerializeField] private string defaultCancelText = "Cancel";

    private void Awake()
    {
        if (cancelButton != null)
        {
            cancelButton.SetActive(false);
        }
    }

    public void Init(UnityAction onOk, string bodyText = null, string okText = null, string cancelText = null, UnityAction onCancel = null)
    {
        okButton.AddOnClick(() => Hide(onOk));

        // Some popups only have ok button 
        if (cancelButton != null && onCancel != null)
        {
            cancelButton.gameObject.SetActive(true);
            cancelButton.SetText(cancelText ?? defaultCancelText);
            cancelButton.AddOnClick(() => Hide(onCancel));
        }

        if (bodyText != null)
        {
            this.bodyText.SetText(bodyText);
        }
        okButton.SetText(okText ?? defaultOkText);
    }

    public void Hide(UnityAction callback)
    {
        callback?.Invoke();
        okButton.onClick.RemoveAllListeners();

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.SetActive(false);
        }
    }
}
