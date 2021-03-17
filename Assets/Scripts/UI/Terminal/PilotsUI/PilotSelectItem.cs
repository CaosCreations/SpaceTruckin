using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilotSelectItem : MonoBehaviour, IPointerClickHandler
{
    private Pilot pilot;
    private MissionsUI missionsUI;
    private MissionScheduleSlot scheduleSlot;
    [SerializeField] private Button itemButton; 
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemImage;

    private void Start()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
    }

    public void Init(Pilot pilot, MissionScheduleSlot scheduleSlot, UnityAction callback)
    {
        this.pilot = pilot;
        this.scheduleSlot = scheduleSlot;
        itemButton.AddOnClick(callback);
        itemText.SetText(BuildItemString());
        itemImage.sprite = pilot.Avatar;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Destroy(gameObject);
            pilot.CurrentMission = null;
            missionsUI.PopulatePilotSelect(scheduleSlot);
        }
    }

    private string BuildItemString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLineWithBreaks(pilot.Name);
        builder.AppendLineWithBreaks(pilot.Ship.Name);
        builder.AppendLineWithBreaks($"Lv. {pilot.Level}");
        return builder.ToString();
    }
}
