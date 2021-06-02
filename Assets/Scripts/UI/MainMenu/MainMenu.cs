public class MainMenu : UICanvasBase
{
    private void OnDisable()
    {
        UIManager.SetCannotInteract();
        UIManager.ResetOverriddenKeys();
    }
}
