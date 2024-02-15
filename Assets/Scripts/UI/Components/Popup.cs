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
        cancelButton.SetActive(false);

    }

    public void Init(UnityAction onOk, string bodyText, string okText = null, string cancelText = null, UnityAction onCancel = null)
    {
        okButton.AddOnClick(() => Hide(onOk));

        // Some popups only have ok button 
        if (onCancel != null)
        {
            cancelButton.gameObject.SetActive(true);
            cancelButton.AddOnClick(() => Hide(onCancel));
        }

        this.bodyText.SetText(bodyText);
        okButton.SetText(okText ?? defaultOkText);
        cancelButton.SetText(cancelText ?? defaultCancelText);
    }

    public void Hide(UnityAction callback)
    {
        callback?.Invoke();
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.SetActive(false);
    }
}
