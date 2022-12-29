using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField]
    private CutsceneContainer cutsceneContainer;

    [SerializeField]
    private PlayableDirector playableDirector;

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
        //RegisterEvents();
    }

    private void RegisterEvents()
    {
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            // Opening station entry cutscene
            if (SceneLoadingManager.GetSceneNameByEnum(Scenes.MainStation) == next.name)
            {
                var onStationLoadCutscene = GetCutsceneByOnLoadSceneName(next.name);

                if (onStationLoadCutscene == null)
                {
                    Debug.LogError("OnStationLoad cutscene not found");
                    return;
                }

                PlayTimeline(onStationLoadCutscene.PlayableAsset);
            }
        };
    }

    public static void PlayCutscene(string cutsceneName)
    {
        var cutscene = Instance.GetCutsceneByName(cutsceneName);

        if (cutscene == null)
            throw new System.Exception("Cannot play cutscene with name: " + cutsceneName);

        PlayTimeline(cutscene.PlayableAsset);
    }

    private static void PlayTimeline(PlayableAsset playableAsset)
    {
        Instance.playableDirector.playableAsset = playableAsset;
        Instance.playableDirector.Play();
    }

    public Cutscene GetCutsceneByName(string name)
    {
        foreach (var cutscene in cutsceneContainer.Elements)
        {
            if (cutscene.Name == name)
                return cutscene;
        }
        Debug.LogError("Cannot find cutscene with name: " + name);
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
