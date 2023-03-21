using UnityEngine;
using UnityEngine.Events;

public class AnimationTimeHandler : MonoBehaviour
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    private bool isRecording;

    [SerializeField]
    private string stateName;

    public UnityAction OnAnimationEnded;

    public void HandleAnimation()
    {
        Animator.Play(stateName);
        isRecording = true;
        Debug.Log($"Animation from {gameObject.name} has finished. State playing: {stateName}.");
    }

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isRecording)
            return;

        if (!Animator.IsPlaying(stateName))
        {
            Debug.Log($"Animation from {gameObject.name} has finished. State played: {stateName}.");
            OnAnimationEnded?.Invoke();
            isRecording = false;
        }
    }
}
