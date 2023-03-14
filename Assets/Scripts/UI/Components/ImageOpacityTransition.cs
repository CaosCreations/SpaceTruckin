using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageOpacityTransition : MonoBehaviour
{
    private Image image;

    private float timer;
    private float opacity;

    [SerializeField]
    private float transitionTime = 4f;

    public UnityAction OnTransitionStart;
    public UnityAction OnTransitionEnd;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        timer = 0;
        opacity = 0;
        OnTransitionStart?.Invoke();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < transitionTime / 2)
        {
            opacity += Time.deltaTime / (transitionTime / 2);
        }
        else
        {
            opacity -= Time.deltaTime / (transitionTime / 2);
        }

        image.color = new Color(0, 0, 0, opacity);

        if (timer >= transitionTime)
        {
            OnTransitionEnd?.Invoke();
            enabled = false;
        }
    }
}