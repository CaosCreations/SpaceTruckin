using Events;
using PixelCrushers.DialogueSystem;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour, ILuaFunctionRegistrar
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField]
    private CutsceneContainer cutsceneContainer;
    [SerializeField]
    private Cutscene openingCutscene;

    [SerializeField]
    private PlayerAnimationAssetMappingContainer playerAnimationAssetMappingContainer;

    private static CutsceneTimelinePlayer[] cutscenePlayers;
    private static CutsceneTimelinePlayer currentCutscenePlayer;

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

    private void RegisterEvents()
    {
        foreach (var cutscenePlayer in cutscenePlayers)
        {
            RegisterEvents(cutscenePlayer.PlayableDirector);
        }
    }

    private void RegisterEvents(PlayableDirector playableDirector)
    {
        playableDirector.played += (_) => OnTimelineStartedHandler();
        playableDirector.stopped += (_) => OnTimelineFinishedHandler();
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    private void OnTimelineStartedHandler()
    {
        PlayerManager.EnterPausedState(false);
        SingletonManager.EventService.Dispatch(new OnCutsceneStartedEvent(currentCutscenePlayer.Cutscene));
    }

    private void OnTimelineFinishedHandler()
    {
        SingletonManager.EventService.Dispatch(new OnCutsceneFinishedEvent(currentCutscenePlayer.Cutscene));
        currentCutscenePlayer = null;
        PlayerManager.ExitPausedState();
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        var convoId = evt.Conversation.id;
        var cutsceneToTrigger = cutsceneContainer.Elements.FirstOrDefault(c => c.ConversationSettings.EndConversationId == convoId);
        if (cutsceneToTrigger != null)
        {
            Debug.Log($"Triggering {cutsceneToTrigger} from end of convo {convoId}.");
            PlayCutscene(cutsceneToTrigger);
        }
    }

    public static void PlayCutscene(Cutscene cutscene)
    {
        if (cutscene == null)
            throw new Exception("Cutscene scriptable object doesn't exist. Cannot play cutscene with name: " + cutscene.Name);

        var cutscenePlayer = Instance.GetCutscenePlayerByCutscene(cutscene);

#pragma warning disable IDE0270 // Use coalesce expression
        if (cutscenePlayer == null)
            throw new Exception("Cutscene player component doesn't exist. Cannot play cutscene with name: " + cutscene.Name);
#pragma warning restore IDE0270 // Use coalesce expression

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
        currentCutscenePlayer.PlayableDirector.Play();
    }

    public Cutscene GetCutsceneByName(string name)
    {
        var insensitiveName = name.RemoveAllWhitespace().ToUpper();
        foreach (var cutscene in cutsceneContainer.Elements)
        {
            if (cutscene.Name.RemoveAllWhitespace().ToUpper() == insensitiveName)
                return cutscene;
        }
        Debug.LogError("Cannot find cutscene with name: " + name);
        return null;
    }

    public CutsceneTimelinePlayer GetCutscenePlayerByCutscene(Cutscene cutscene)
    {
        foreach (var cutscenePlayer in cutscenePlayers)
        {
            if (cutscenePlayer.Cutscene == cutscene)
                return cutscenePlayer;
        }
        Debug.LogError("CutsceneTimelinePlayer not found by " + cutscene);
        return null;
    }

    public void SetUp()
    {
        cutscenePlayers = FindObjectsOfType<CutsceneTimelinePlayer>();
        foreach (var cutscenePlayer in cutscenePlayers)
        {
            //SetUpDirectorBindings(cutscenePlayer.PlayableDirector);
            //SetUpAnimationTracks(cutscenePlayer.PlayableDirector);
        }
        RegisterEvents();
        PlayCutscene(openingCutscene);
    }

    private void SetUpDirectorBindings(PlayableDirector playableDirector)
    {
        if (!PlayerManager.PlayerObject.TryGetComponent<Animator>(out var playerAnimator))
        {
            throw new Exception("Unable to get player animator to bind to Timeline director");
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
            throw new Exception("Unable to get player animator to set animation tracks");

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
        if (currentCutscenePlayer == null)
            return;

        if (currentCutscenePlayer.PlayableDirector.playableAsset != null && currentCutscenePlayer.PlayableDirector.time > 0)
            currentCutscenePlayer.PlayableDirector.time = 100000;
    }

    public void RegisterLuaFunctions()
    {
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
