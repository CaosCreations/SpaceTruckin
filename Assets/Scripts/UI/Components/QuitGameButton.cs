using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.AddOnClick(Quit);
    }

    private void Quit()
    {
        Debug.Log("Quitting game...");
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else if (Application.isPlaying)
        {
            Application.Quit();
        }
    }
}
