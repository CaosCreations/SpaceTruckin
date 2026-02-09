// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Maps an actor name to a GameObject that will replace that
    /// actor's role in a conversation.
    /// </summary>
    [System.Serializable]
    public class ActorOverride
    {
        [ActorPopup] public string actor;
        public Transform replaceWith;
    }

}
