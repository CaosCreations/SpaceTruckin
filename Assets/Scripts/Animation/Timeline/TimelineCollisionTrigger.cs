using UnityEngine;
using UnityEngine.Playables;

public class TimelineCollisionTrigger : MonoBehaviour
{
    [SerializeField] protected PlayableDirector playableDirector;

    protected virtual void Awake() { }

    protected virtual void PlayTimeline()
    {
        playableDirector.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            PlayTimeline();
        }
    }
}
