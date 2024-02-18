﻿using Events;
using PixelCrushers.DialogueSystem;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour, ILuaFunctionRegistrar
{
    public static TimelineManager Instance { get; private set; }

    [SerializeField] private CutsceneContainer cutsceneContainer;
    [SerializeField] private Cutscene openingCutscene;
    [SerializeField] private PlayerAnimationAssetMappingContainer playerAnimationAssetMappingContainer;
    [SerializeField] private GameObject pauseView;

    private static CutsceneTimelinePlayer[] cutscenePlayers;
    private static CutsceneTimelinePlayer currentCutscenePlayer;
    public static bool IsPlaying { get; private set; }
    private static bool isPaused;
    private double pausedTime;

    private AnimationMixerPlayable mixer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        pauseView.SetActive(false);
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
        IsPlaying = true;
        PlayerManager.EnterPausedState(false);
        UIManager.AddOverriddenKey(KeyCode.Escape);
        SingletonManager.EventService.Dispatch(new OnCutsceneStartedEvent(currentCutscenePlayer.Cutscene));
    }

    private void OnTimelineFinishedHandler()
    {
        IsPlaying = false;
        var cutscene = currentCutscenePlayer.Cutscene;
        var animatorSettings = currentCutscenePlayer.AnimatorSettings;

        if (!string.IsNullOrWhiteSpace(animatorSettings.StateOnEnd))
        {
            animatorSettings.Animator.Play(animatorSettings.StateOnEnd);
        }
        UIManager.RemoveOverriddenKey(KeyCode.Escape);
        pauseView.SetActive(false);
        isPaused = false;

        SingletonManager.EventService.Dispatch(new OnCutsceneFinishedEvent(cutscene));
        currentCutscenePlayer = null;
        mixer = default;

        // TODO: Maybe put this before notifying subscribers 
        if (!cutscene.ConversationSettings.DontUnpausePlayerOnEnd)
        {
            PlayerManager.ExitPausedState();
        }

        if (cutscene.CutsceneOnEnd != null)
        {
            PlayCutscene(cutscene.CutsceneOnEnd);
        }
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

        if (cutscenePlayer == null)
            throw new Exception("Cutscene player component doesn't exist. Cannot play cutscene with name: " + cutscene.Name);

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
        //foreach (var cutscenePlayer in cutscenePlayers)
        //{
        //    SetUpDirectorBindings(cutscenePlayer.PlayableDirector);
        //    SetUpAnimationTracks(cutscenePlayer.PlayableDirector);
        //}
        RegisterEvents();
        PlayCutscene(openingCutscene);
    }

    //private void SetUpDirectorBindings(PlayableDirector playableDirector)
    //{
    //    if (!PlayerManager.PlayerObject.TryGetComponent<Animator>(out var playerAnimator))
    //    {
    //        throw new Exception("Unable to get player animator to bind to Timeline director");
    //    }

    //    foreach (var binding in playableDirector.playableAsset.outputs)
    //    {
    //        if (binding.streamName == "playerAnim" || binding.streamName == "Animation Track")
    //        {
    //            Debug.Log($"Setting binding with name: '{binding.streamName}' to player animator object");
    //            playableDirector.SetGenericBinding(binding.sourceObject, playerAnimator);
    //        }
    //    }
    //}

    //private void SetUpAnimationTracks(PlayableDirector playableDirector)
    //{
    //    if (!PlayerManager.PlayerObject.TryGetComponent<Animator>(out var playerAnimator))
    //        throw new Exception("Unable to get player animator to set animation tracks");

    //    var timelineAsset = playableDirector.playableAsset as TimelineAsset;

    //    foreach (var track in timelineAsset.GetOutputTracks())
    //    {
    //        var animationTrack = track as AnimationTrack;

    //        if (animationTrack == null)
    //            continue;

    //        // Match and set the player clips on each of the animation assets in the timeline 
    //        foreach (var clip in animationTrack.GetClips())
    //        {
    //            var animationAsset = clip.asset as AnimationPlayableAsset;

    //            if (animationAsset == null)
    //                continue;

    //            var assetMapping = playerAnimationAssetMappingContainer.GetMappingByClipName(animationAsset.clip.name);

    //            if (assetMapping == null)
    //                continue;

    //            animationAsset.clip = playerAnimator.runtimeAnimatorController.name == AnimationConstants.Player1ControllerName
    //                ? assetMapping.Player1Clip
    //                : assetMapping.Player2Clip;
    //        }
    //    }
    //}

    public void FinishCurrentTimeline()
    {
        if (currentCutscenePlayer == null)
            return;

        if (currentCutscenePlayer.PlayableDirector.playableAsset != null && currentCutscenePlayer.PlayableDirector.time > 0)
            currentCutscenePlayer.PlayableDirector.time = 100000;
    }

    public void PauseTimeline()
    {
        if (currentCutscenePlayer == null)
            return;

        //currentCutscenePlayer.PlayableDirector.Pause();
        //var asset = currentCutscenePlayer.PlayableDirector.playableAsset as TimelineAsset;
        //mixer = AnimationMixerPlayable.Create(currentCutscenePlayer.PlayableDirector.playableGraph, asset.outputTrackCount);
        //currentCutscenePlayer.PlayableDirector.playableGraph.GetOutput(0).GetSourcePlayable().AddInput(mixer, 0);
        //mixer.SetSpeed(0f);

        pausedTime = currentCutscenePlayer.PlayableDirector.time;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeTimeline()
    {
        if (currentCutscenePlayer == null)
            return;

        //currentCutscenePlayer.PlayableDirector.Resume();
        //mixer.SetSpeed(1f);
        isPaused = false;
        Time.timeScale = 1f;
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

    private void Update()
    {
        if (!IsPlaying)
            return;

        //if (isPaused)
        //    currentCutscenePlayer.PlayableDirector.time = pausedTime;

        if (Input.GetKeyDown(PlayerConstants.PauseKey))
        {
            if (isPaused)
            {
                ResumeTimeline();
            }
            else
            {
                PauseTimeline();
            }
            pauseView.SetActive(isPaused);
        }
    }
}
