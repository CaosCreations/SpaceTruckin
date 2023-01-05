using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField]
    private CutsceneContainer cutsceneContainer;

    [SerializeField]
    private PlayableDirector playableDirector;

    [SerializeField]
    private PlayerAnimationClipData playerClipData;

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
        Instance.playableDirector.gameObject.SetActive(true);
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

    public void SetupDirector()
    {
        SetDirectorBindings();
        SetAnimationTracks();
    }

    private void SetDirectorBindings()
    {
        if (!PlayerManager.PlayerObject.TryGetComponent<Animator>(out var playerAnimator))
        {
            throw new System.Exception("Unable to get player animator to bind to Timeline director");
        }

        foreach (var binding in playableDirector.playableAsset.outputs)
        {
            if (binding.streamName == "playerAnim" || binding.streamName == "Animation Track")
            {
                Debug.Log($"Setting binding with name: '{binding.streamName}' to player animator object");
                playableDirector.SetGenericBinding(binding.sourceObject, playerAnimator);
            }
        }
    }

    private void SetAnimationTracks()
    {
        if (!PlayerManager.PlayerObject.TryGetComponent<Animator>(out var playerAnimator))
            throw new System.Exception("Unable to get player animator to set animation tracks");

        if (playerAnimator.runtimeAnimatorController.name != "SprPlayer2newAnim")
            return;

        var timelineAsset = playableDirector.playableAsset as TimelineAsset;

        foreach (var track in timelineAsset.GetOutputTracks())
        {
            var animationTrack = track as AnimationTrack;

            if (animationTrack == null)
                continue;

            // Match and set the player clips on each of the animation assets in the timeline 
            foreach (var clips in animationTrack.GetClips())
            {
                var animationAsset = clips.asset as AnimationPlayableAsset;

                if (animationAsset == null || !animationClipMap.TryGetValue(animationAsset.clip.name, out var mappedClipName))
                    continue;

                var mappedClip = playerClipData.Player2AnimationClips.FirstOrDefault(clip => clip != null && clip.name == mappedClipName);

                if (mappedClip == null)
                    continue;

                animationAsset.clip = mappedClip;
            }
        }
    }

    /// <summary>
    ///     P1 to P2 mappings of animation clip names.
    /// </summary>
    private static readonly Dictionary<string, string> animationClipMap = new Dictionary<string, string>
    {
        { "WalkLeft", "player2_walkLeft" },
        { "StandDownP", "player2_IdleDown" },
        { "RunDown", "player2_runDown" },
        { "RunLeft", "player2_runLeft" },
        { "RunUP", "player2_runUp" },
        { "StandUpP", "player2_IdleUp" }
    };
}
