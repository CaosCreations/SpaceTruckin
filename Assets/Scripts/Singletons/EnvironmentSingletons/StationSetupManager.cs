using UnityEngine;
using UnityEngine.SceneManagement;

public class StationSetupManager : MonoBehaviour
{
    public static StationSetupManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            // When changing to main station scene 
            if (SceneLoadingManager.GetSceneNameByEnum(Scenes.MainStation) == next.name)
            {
                Debug.Log("Setting up station...");
                SetupStation();
            }
        };
    }

    public static void SetupStation()
    {
        PlayerManager.Instance.SetupPlayer();
        TimelineManager.Instance.SetupDirector();
        TimelineManager.PlayCutscene("Opening Cutscene");
    }
}