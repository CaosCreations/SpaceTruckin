using Events;
using System.Linq;
using UnityEngine;

public class TemporaryConditionalObjectManager : MonoBehaviour
{
    private TemporaryConditionalObject[] objects;

    // Date night hack 
    [SerializeField] private GameObject workshopDoors;

    private void Start()
    {
        objects = FindObjectsOfType<TemporaryConditionalObject>(true);
        Debug.Log($"{nameof(TemporaryConditionalObjectManager)} - found objects: {string.Join(", ", objects.Select(o => o.name))}");
        ConditionallySetActive();

        if (SingletonManager.Instance != null)
        {
            SingletonManager.EventService.Add<OnEndOfDayEvent>(EndOfDayHandler);
            SingletonManager.EventService.Add<OnEveningStartEvent>(EveningStartHandler);
            SingletonManager.EventService.Add<OnMessageReadEvent>(MessageReadHandler);
            SingletonManager.EventService.Add<OnMissionCompletedEvent>(MissionCompletedHandler);
            SingletonManager.EventService.Add<OnConversationEndedEvent>(ConversationEndedHandler);
            SingletonManager.EventService.Add<OnDialogueVariableUpdatedEvent>(DialogueVariableUpdatedHandler);
        }
    }

    private void ConditionallySetActive()
    {
        foreach (var obj in objects)
        {
            obj.ConditionallySetActive();
        }
    }

    private void EndOfDayHandler(OnEndOfDayEvent evt)
    {
        ConditionallySetActive();
        workshopDoors.transform.SetDirectChildrenActive(false);
    }

    private void EveningStartHandler()
    {
        ConditionallySetActive();

        // Hack for now 
        var missionCompleted = MissionsManager.Instance.HasMissionBeenCompleted("Custom Nav Computer");
        var dateNightActive = DialogueDatabaseManager.GetLuaVariableAsBool("Date_Night_active");

        var isDateNight = CalendarManager.CurrentDate >= new Date(2, 1, 1) && missionCompleted && dateNightActive;
        workshopDoors.transform.SetDirectChildrenActive(!isDateNight);
    }

    private void MessageReadHandler()
    {
        ConditionallySetActive();
    }

    private void MissionCompletedHandler()
    {
        ConditionallySetActive();
    }

    private void ConversationEndedHandler(OnConversationEndedEvent evt)
    {
        ConditionallySetActive();
    }

    private void DialogueVariableUpdatedHandler()
    {
        ConditionallySetActive();
    }
}
