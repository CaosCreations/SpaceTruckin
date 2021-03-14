using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PilotSelectItem : MonoBehaviour
{
    public Pilot pilot;
    public Button itemButton; 
    public Text itemText;
    public Image itemImage; 
    
    public void Init(Pilot pilot, UnityAction callback)
    {
        this.pilot = pilot;
        itemButton.AddOnClick(callback);
        itemText.SetText($"{pilot.Name} (Lv. {pilot.Level})");
        itemImage.sprite = pilot.Avatar;
    }
}
