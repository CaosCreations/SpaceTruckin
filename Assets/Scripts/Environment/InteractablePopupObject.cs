using UnityEngine;

public class InteractablePopupObject : InteractableObject
{
    [SerializeField] private PopupType popupType = PopupType.Default;
    [SerializeField] private bool destroyOnUse;
    [SerializeField] private string text;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(PlayerConstants.ActionKey) && IsPlayerInteractable)
        {
            PopupManager.ShowPopup(bodyText: text, type: popupType);

            if (destroyOnUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
