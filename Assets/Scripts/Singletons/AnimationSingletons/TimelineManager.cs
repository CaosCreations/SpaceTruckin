using Cinemachine;
using Events;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour, ILuaFunctionRegistrar
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField]
    private CutsceneContainer cutsceneContainer;

    [SerializeField]
    private PlayableDirector playableDirector;

    [SerializeField]
    private CinemachineVirtualCamera cutsceneCamera;

    [SerializeField]
    private PlayerAnimationAssetMappingContainer playerAnimationAssetMappingContainer;

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
        playableDirector.played += (_) => OnTimelineStartedHandler();
        playableDirector.stopped += (_) => OnTimelineFinishedHandler();
    }

    private void OnTimelineStartedHandler()
    {
        cutsceneCamera.Priority = TimelineConstants.CutsceneCameraPlayPriority;

        var cutscene = GetCutsceneByPlayableAsset(playableDirector.playableAsset);
        if (cutscene != null)
        {
            SingletonManager.EventService.Dispatch(new OnCutsceneStartedEvent(cutscene));
        }
        else
        {
            SingletonManager.EventService.Dispatch(new OnTimelineStartedEvent(playableDirector.playableAsset));
        }
    }

    private void OnTimelineFinishedHandler()
    {
        cutsceneCamera.Priority = TimelineConstants.CutsceneCameraBasePriority;

        var cutscene = GetCutsceneByPlayableAsset(playableDirector.playableAsset);
        if (cutscene != null)
        {
            SingletonManager.EventService.Dispatch(new OnCutsceneFinishedEvent(cutscene));
        }
        else
        {
            SingletonManager.EventService.Dispatch(new OnTimelineFinishedEvent(playableDirector.playableAsset));
        }
    }

    public static void PlayCutscene(string cutsceneName)
    {
        var cutscene = Instance.GetCutsceneByName(cutsceneName);

        if (cutscene == null)
            throw new System.Exception("Cannot play cutscene with name: " + cutsceneName);

        Debug.Log("Playing cutscene with name: " + cutsceneName);
        PlayTimeline(cutscene.PlayableAsset);
    }

    private static void PlayTimeline(PlayableAsset playableAsset)
    {
        Debug.Log("Playing timeline with playable asset name: " + playableAsset.name);
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

    public Cutscene GetCutsceneByPlayableAsset(PlayableAsset playableAsset)
    {
        foreach (var cutscene in cutsceneContainer.Elements)
        {
            if (cutscene.PlayableAsset == playableAsset)
                return cutscene;
        }
        Debug.LogError("Cannot find cutscene with playable asset: " + playableAsset);
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

    public void SetUp()
    {
        SetUpDirectorBindings();
        SetUpAnimationTracks();
        SetUpCutsceneCamera();
    }

    private void SetUpDirectorBindings()
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

    private void SetUpAnimationTracks()
    {
        if (!PlayerManager.PlayerObject.TryGetComponent<Animator>(out var playerAnimator))
            throw new System.Exception("Unable to get player animator to set animation tracks");

        var timelineAsset = playableDirector.playableAsset as TimelineAsset;

        foreach (var track in timelineAsset.GetOutputTracks())
        {
            var animationTrack = track as AnimationTrack;

            if (animationTrack == null)
                continue;

            // Match and set the player clips on each of the animation assets in the timeline 
            foreach (var clip in animationTrack.GetClips())
            {
                var animationAsset = clip.asset as AnimationPlayableAsset;

                if (animationAsset == null)
                    continue;

                var assetMapping = playerAnimationAssetMappingContainer.GetMappingByClipName(animationAsset.clip.name);

                if (assetMapping == null)
                    continue;

                animationAsset.clip = playerAnimator.runtimeAnimatorController.name == AnimationConstants.Player1ControllerName
                    ? assetMapping.Player1Clip
                    : assetMapping.Player2Clip;
            }
        }
    }

    private void SetUpCutsceneCamera()
    {
        cutsceneCamera.Follow = PlayerManager.PlayerObject.transform;
    }

    public void FinishCurrentTimeline()
    {
        if (playableDirector.playableAsset != null && playableDirector.time > 0)
            playableDirector.time = 100000; // Some unrealistically high value
    }

    public void RegisterLuaFunctions()
    {
        if (DialogueManager.Instance == null)
            return;

        Lua.RegisterFunction(
            DialogueConstants.PlayCutsceneFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => PlayCutscene(string.Empty)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.PlayCutsceneFunctionName);
    }
}
