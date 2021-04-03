using UnityEngine;
using UnityEngine.UI;

public class ShipDetails : MonoBehaviour
{
    [SerializeField] private Text shipNameText;
    [SerializeField] private Image shipAvatarImage;

    public void Init(Ship ship)
    {
        if (ship != null)
        {
            shipNameText.SetText(ship.Name);
            shipAvatarImage.sprite = ship.Avatar;
        }
    }
}
