using Events;
using System.Linq;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField]
    private Cutscene[] endOfDayCutscenes;

    private void Start()
    {
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    public static void Sleep()
    {
        PlayerManager.EnterPausedState();
        UIManager.ClearCanvases(false);
        SingletonManager.EventService.Dispatch(new OnPlayerSleepEvent());
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (endOfDayCutscenes != null && endOfDayCutscenes.Contains(evt.Cutscene) && CalendarManager.DateIsToday(evt.Cutscene.Date))
        {
            Sleep();
        }
    }
}
