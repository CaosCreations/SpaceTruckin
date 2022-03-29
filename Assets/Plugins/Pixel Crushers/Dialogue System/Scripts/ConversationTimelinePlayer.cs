using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ConversationTimelinePlayer : MonoBehaviour
{
    public static ConversationTimelinePlayer Instance;

    private PlayableDirector playableDirector;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayPlayableAsset(PlayableAsset playableAsset)
    {
        playableDirector.playableAsset = playableAsset;

        playableDirector.Play();
    }
}
