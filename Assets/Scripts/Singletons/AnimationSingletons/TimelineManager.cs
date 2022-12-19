using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField]
    private CutsceneContainer cutsceneContainer;

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

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            if (SceneLoadingManager.GetSceneNameByEnum(Scenes.TitleScreen) == next.name)
            {
                var onStationLoadCutscene = GetCutsceneByOnLoadSceneName(next.name);

                if (onStationLoadCutscene == null)
                {
                    Debug.LogError("OnStationLoad cutscene not found");
                    return;
                }

                PlayTimeline(onStationLoadCutscene.Director);
            }
        };
    }

    public static void PlayTimeline(PlayableDirector playableDirector)
    {
        playableDirector.Play();
    }

    public Cutscene GetCutsceneByName(string name)
    {
        foreach (var cutscene in cutsceneContainer.Elements)
        {
            if (cutscene.Name == name)
                return cutscene;
        }
        return null;
    }

    public Cutscene GetCutsceneByOnLoadSceneName(string onSceneLoadName)
    {
        foreach (var cutscene in cutsceneContainer.Elements)
        {
            if (cutscene.OnSceneLoadName == onSceneLoadName)
                return cutscene;
        }
        return null;
    }
}
