#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class SequencerMessageClip : PlayableAsset, ITimelineClipAsset
    {
        public SequencerMessageBehaviour template = new SequencerMessageBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SequencerMessageBehaviour>.Create(graph, template);
            SequencerMessageBehaviour clone = playable.GetBehaviour();
            return playable;
        }
    }
}
#endif
#endif
