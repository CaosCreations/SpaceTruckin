using Events;

public class BedCanvasUI : UICanvasBase
{
    private void OnEnable()
    {
        UIManager.ClearCanvases(false);
        SingletonManager.EventService.Dispatch(new OnPlayerSleepEvent());
    }
}
