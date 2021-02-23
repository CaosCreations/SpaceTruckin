using UnityEngine;

public class FontManager : MonoBehaviour
{
    public static FontManager Instance { get; private set; }
    public Font Aero;
    public Font AlienLeague;
    public Font HemiHead;
    public Font Optimus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
