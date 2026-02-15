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
    [SerializeField] private Material dissolveMaterial;

    private static readonly int DissolveProperty = Shader.PropertyToID("_Dissolve");
    private static readonly int EdgeColorProperty = Shader.PropertyToID("_EdgeColor");

    private Coroutine animationCoroutine;
    private Material runtimeMaterial;

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

        runtimeMaterial = Instantiate(dissolveMaterial);
        runtimeMaterial.SetFloat(DissolveProperty, 0f);
        runtimeMaterial.SetColor(EdgeColorProperty, data.DissolveEdgeColor);
        image.material = runtimeMaterial;

        var color = image.color;
        color.a = 1f;
        image.color = color;

        image.sprite = frames[0];

        var dissolveTween = DOTween.To(
                () => runtimeMaterial.GetFloat(DissolveProperty),
                value => runtimeMaterial.SetFloat(DissolveProperty, value),
                1f,
                data.DissolveDuration)
            .SetUpdate(true);

        if (data.ScalePunchStrength > 0f)
        {
            var rectTransform = image.rectTransform;
            rectTransform.DOPunchScale(
                    Vector3.one * data.ScalePunchStrength,
                    data.DissolveDuration,
                    vibrato: 6,
                    elasticity: 0.5f)
                .SetUpdate(true);
        }

        if (data.IntroShake != null && data.IntroShake.Amplitude > 0f)
        {
            StationCameraManager.ShakeLiveCamera(data.IntroShake);
        }

        var framesCoroutine = StartCoroutine(PlayFrames(frames, frameRate));

        yield return dissolveTween.WaitForCompletion();
        yield return framesCoroutine;

        yield return new WaitForSecondsRealtime(data.HoldDuration);

        Finish(data);
    }

    private IEnumerator PlayFrames(Sprite[] frames, float frameRate)
    {
        for (var i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            yield return new WaitForSecondsRealtime(frameRate);
        }
    }

    private void Finish(ActorIntroAnimationData data)
    {
        image.material = null;

        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
            runtimeMaterial = null;
        }

        image.rectTransform.localScale = Vector3.one;

        canvas.gameObject.SetActive(false);
        IsPlaying = false;
        animationCoroutine = null;

        UIManager.RemoveOverriddenKey(KeyCode.Escape);

        SingletonManager.EventService.Dispatch(new OnActorIntroAnimationFinishedEvent(data));

        OnAnimationFinished?.Invoke();
    }
}
