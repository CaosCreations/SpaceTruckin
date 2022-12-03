using UnityEngine;
using UnityEngine.EventSystems;

public class DemoSpriteArrow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private DemoSpritePicker spritePicker;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        UISoundEffectsManager.Instance.PlaySoundEffect(UISoundEffect.Confirm);
        spritePicker.PickSprite();
    }
}
