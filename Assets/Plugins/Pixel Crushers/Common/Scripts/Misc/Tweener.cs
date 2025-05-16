using System.Collections;
using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// General purpose tweener.
    ///
    /// Example: fading out audio source volume over 0.2 seconds:
    /// var audioSource = GetComponent<AudioSource>();
    /// Tweener.Tween(1, 0, 0.2f, false, Tweener.Easing.Linear,
    ///     onBegin: null,
    ///     onValue: (x) => audioSource.volume = x,
    ///     onEnd: null);
    /// </summary>
    public class Tweener : MonoBehaviour
    {

        public enum Easing
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic
        }

        private static Tweener instance;

        public static Tweener Instance
        {
            get
            {
                if (instance == null) instance = new GameObject("Tweener").AddComponent<Tweener>();
                return instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void SubsystemRegistration()
        {
            instance = null;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static Coroutine Tween(float from, float to, float seconds,
            bool unscaledTime, Easing easing,
            System.Action onBegin, System.Action<float> onValue, System.Action onEnd)
        {
            return Instance.StartCoroutine(Instance.TweenCoroutine(from, to, seconds,
                unscaledTime, easing,
                (f, t, c) => Mathf.Lerp(f, t, c),
                onBegin, onValue, onEnd));
        }

        public static Coroutine Tween(Vector2 from, Vector2 to, float seconds,
            bool unscaledTime, Easing easing,
            System.Action onBegin, System.Action<Vector2> onValue, System.Action onEnd)
        {
            return Instance.StartCoroutine(Instance.TweenCoroutine(from, to, seconds,
                unscaledTime, easing,
                (f, t, c) => Vector2.Lerp(f, t, c),
                onBegin, onValue, onEnd));
        }

        public static Coroutine Tween(Vector3 from, Vector3 to, float seconds,
            bool unscaledTime, Easing easing,
            System.Action onBegin, System.Action<Vector3> onValue, System.Action onEnd)
        {
            return Instance.StartCoroutine(Instance.TweenCoroutine(from, to, seconds,
                unscaledTime, easing,
                (f, t, c) => Vector3.Lerp(f, t, c),
                onBegin, onValue, onEnd));
        }

        public static Coroutine Tween(Quaternion from, Quaternion to, float seconds,
            bool unscaledTime, Easing easing,
            System.Action onBegin, System.Action<Quaternion> onValue, System.Action onEnd)
        {
            return Instance.StartCoroutine(Instance.TweenCoroutine(from, to, seconds,
                unscaledTime, easing,
                (f, t, c) => Quaternion.Lerp(f, t, c),
                onBegin, onValue, onEnd));
        }

        public static Coroutine Tween(Color from, Color to, float seconds,
            bool unscaledTime, Easing easing,
            System.Action onBegin, System.Action<Color> onValue, System.Action onEnd)
        {
            return Instance.StartCoroutine(Instance.TweenCoroutine(from, to, seconds,
                unscaledTime, easing,
                (f, t, c) => Color.Lerp(f, t, c),
                onBegin, onValue, onEnd));
        }

        private delegate T LerpFunction<T>(T from, T to, float current);

        private IEnumerator TweenCoroutine<T>(T from, T to, float seconds,
            bool unscaledTime, Easing easing, LerpFunction<T> lerpFunction,
            System.Action onBegin, System.Action<T> onValue, System.Action onEnd)
        {
            onBegin?.Invoke();
            if (onValue != null)
            {
                onValue.Invoke(from);
                float elapsed = 0;
                while (elapsed < seconds)
                {
                    var t = elapsed / seconds;
                    var current = GetEasingValue(easing, t);
                    onValue(lerpFunction(from, to, current));
                    yield return null;
                    elapsed += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                }
                onValue.Invoke(to);
            }
            onEnd?.Invoke();
        }

        private float GetEasingValue(Easing easing, float t)
        {
            const float c4 = (2 * Mathf.PI) / 3f;
            const float c5 = (2 * Mathf.PI) / 4.5f;
            switch (easing)
            {
                default:
                case Easing.Linear:
                    return t;
                case Easing.EaseIn:
                    return t * t;
                case Easing.EaseOut:
                    return t * (2f - t);
                case Easing.EaseInOut:
                    if ((t *= 2f) < 1f) return 0.5f * t * t;
                    return -0.5f * ((t -= 1f) * (t - 2f) - 1f);
                case Easing.EaseInElastic:
                    if (t == 0) return 0;
                    if (t == 1) return 1;
                    return -Mathf.Pow(2, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * c4);
                case Easing.EaseOutElastic:
                    if (t == 0) return 0;
                    if (t == 1) return 1;
                    return Mathf.Pow(2, -10f * t) * Mathf.Sin((t * 10f - 10.75f) * c4) + 1;
                case Easing.EaseInOutElastic:
                    if (t == 0) return 0;
                    if (t == 1) return 1;
                    if (t < 0.5f) return -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2;
                    return (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;

            }
        }

    }

}
