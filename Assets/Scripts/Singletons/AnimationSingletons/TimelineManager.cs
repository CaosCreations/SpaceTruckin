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
    private PlayerAnimationAssetMappingContainer playerAnimationAssetMappingContainer;

    private static CutsceneTimelinePlayer[] cutscenePlayers;
    private static CutsceneTimelinePlayer currentCutscenePlayer;

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

        cutscenePlayers = GetComponentsInChildren<CutsceneTimelinePlayer>(true);
    }

    private void Start()
    {
        RegisterLuaFunctions();

        foreach (var cutscenePlayer in cutscenePlayers)
        {
            RegisterEvents(cutscenePlayer.PlayableDirector);
        }
    }

    private void RegisterEvents(PlayableDirector playableDirector)
    {
        playableDirector.played += (_) => OnTimelineStartedHandler();
        playableDirector.stopped += (_) => OnTimelineFinishedHandler();
    }

    private void OnTimelineStartedHandler()
    {
        PlayerManager.EnterPausedState(false);
        currentCutscenePlayer.VirtualCamera.Priority = TimelineConstants.CutsceneCameraPlayPriority;
        currentCutscenePlayer.VirtualCamera.Follow = PlayerManager.PlayerObject.transform;

        var cutscene = GetCutsceneByPlayableAsset(currentCutscenePlayer.PlayableDirector.playableAsset);
        if (cutscene != null)
        {
            SingletonManager.EventService.Dispatch(new OnCutsceneStartedEvent(cutscene));
        }
        else
        {
            SingletonManager.EventService.Dispatch(new OnTimelineStartedEvent(currentCutscenePlayer.PlayableDirector.playableAsset));
        }
    }

    private void OnTimelineFinishedHandler()
    {
        currentCutscenePlayer.VirtualCamera.Priority = TimelineConstants.CutsceneCameraBasePriority;
        currentCutscenePlayer.VirtualCamera.Follow = null;
        currentCutscenePlayer.VirtualCamera.gameObject.SetActive(false);

        var cutscene = GetCutsceneByPlayableAsset(currentCutscenePlayer.PlayableDirector.playableAsset);
        if (cutscene != null)
        {
            SingletonManager.EventService.Dispatch(new OnCutsceneFinishedEvent(cutscene));
        }
        else
        {
            SingletonManager.EventService.Dispatch(new OnTimelineFinishedEvent(currentCutscenePlayer.PlayableDirector.playableAsset));
        }

        currentCutscenePlayer = null;
        PlayerManager.ExitPausedState();
    }

    public static void PlayCutscene(Cutscene cutscene)
    {
        if (cutscene == null)
            throw new System.Exception("Cutscene scriptable object doesn't exist. Cannot play cutscene with name: " + cutscene.Name);

        var cutscenePlayer = Instance.GetCutscenePlayerByCutscene(cutscene);

        if (cutscenePlayer == null)
            throw new System.Exception("Cutscene player component doesn't exist. Cannot play cutscene with name: " + cutscene.Name);

        currentCutscenePlayer = cutscenePlayer;

        Debug.Log("Playing cutscene with name: " + cutscene.Name);
        PlayTimeline();
    }

    public static void PlayCutscene(string cutsceneName)
    {
        var cutscene = Instance.GetCutsceneByName(cutsceneName);
        PlayCutscene(cutscene);
    }

    private static void PlayTimeline()
    {
        Debug.Log("Playing timeline with playable asset name: " + currentCutscenePlayer.PlayableDirector.playableAsset.name);
        currentCutscenePlayer.PlayableDirector.gameObject.SetActive(true);
        currentCutscenePlayer.VirtualCamera.gameObject.SetActive(true);
        currentCutscenePlayer.PlayableDirector.Play();
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

    public CutsceneTimelinePlayer GetCutscenePlayerByCutscene(Cutscene cutscene)
    {
        foreach (var cutscenePlayer in cutscenePlayers)
        {
            if (cutscenePlayer.Cutscene == cutscene)
                return cutscenePlayer;
        }
        return null;
    }

    public void SetUp()
    {
        foreach (var cutscenePlayer in cutscenePlayers)
        {
            SetUpDirectorBindings(cutscenePlayer.PlayableDirector);
            SetUpAnimationTracks(cutscenePlayer.PlayableDirector);
        }
    }

    private void SetUpDirectorBindings(PlayableDirector playableDirector)
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

    private void SetUpAnimationTracks(PlayableDirector playableDirector)
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

    public void FinishCurrentTimeline()
    {
        if (currentCutscenePlayer.PlayableDirector.playableAsset != null && currentCutscenePlayer.PlayableDirector.time > 0)
            currentCutscenePlayer.PlayableDirector.time = 100000;
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
