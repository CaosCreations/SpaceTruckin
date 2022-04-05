using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Timeline conversation asset", order = 1)]
public class TimelineConversationAsset : ScriptableObject
{
    [SerializeField] private PlayableAsset playableAsset;

    public void PlayPlayableAsset()
    {
        ConversationTimelinePlayer.Instance.PlayPlayableAsset(playableAsset);
    }
}
