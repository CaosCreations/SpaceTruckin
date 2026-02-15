using System;
using System.Collections;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class ActorIntroAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;

    private Coroutine animationCoroutine;

    public bool IsPlaying { get; private set; }

    public event Action OnAnimationFinished;

    public void Play(ActorIntroAnimationData data)
    {
        if (data == null || data.Frames == null || data.Frames.Length == 0)
        {
            Debug.LogWarning("ActorIntroAnimationPlayer: No frames to play.");
            OnAnimationFinished?.Invoke();
            return;
        }

        canvas.gameObject.SetActive(true);
        IsPlaying = true;

        PlayerManager.EnterPausedState();
        UIManager.AddOverriddenKey(KeyCode.Escape);

        SingletonManager.EventService.Dispatch(new OnActorIntroAnimationStartedEvent(data));

        animationCoroutine = StartCoroutine(PlayAnimation(data));
    }

    private IEnumerator PlayAnimation(ActorIntroAnimationData data)
    {
        var frames = data.Frames;
        var frameRate = data.FrameRate;

        var color = image.color;
        color.a = 0f;
        image.color = color;

        image.sprite = frames[0];
        var fadeTween = image.DOFade(1f, data.FadeInDuration).SetUpdate(true);
        yield return fadeTween.WaitForCompletion();

        for (var i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            yield return new WaitForSecondsRealtime(frameRate);
        }

        yield return new WaitForSecondsRealtime(data.HoldDuration);

        Finish(data);
    }

    private void Finish(ActorIntroAnimationData data)
    {
        canvas.gameObject.SetActive(false);
        IsPlaying = false;
        animationCoroutine = null;

        UIManager.RemoveOverriddenKey(KeyCode.Escape);

        SingletonManager.EventService.Dispatch(new OnActorIntroAnimationFinishedEvent(data));

        OnAnimationFinished?.Invoke();
    }
}
