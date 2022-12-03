using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameHUDManager : MonoBehaviour
{
    public static InGameHUDManager Instance { get; private set; }

    [SerializeField]
    private GameObject hudCanvas;

    [SerializeField]
    private DateTimeText dateTimeText;

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
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            bool isActive = next.name == SceneLoadingManager.GetSceneNameByEnum(Scenes.MainStation);
            SetActive(isActive);
        };
    }

    private static void SetActive(bool active)
    {
        if (Instance.hudCanvas == null)
        {
            Debug.LogWarning("Can't set HUD canvas object to active/inactive because it is null.");
            return;
        }

        Instance.hudCanvas.SetActive(active);
        Instance.dateTimeText.SetActive(active);
    }
}
