#if USE_TIMELINE
// Copyright (c) Pixel Crushers. All rights reserved.
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Add this component to a subtitle panel's continue button to change its behavior
    /// so it hides a subtitle panel and sends a sequencer message. Typically used
    /// in timelines to unpause the timeline.
    /// Unassign the Button component's OnClick() event.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class TimelineContinueButton : MonoBehaviour
    {

        [Tooltip("Hide this subtitle panel when continue button is clicked.")]
        public StandardUISubtitlePanel subtitlePanel;
        [Tooltip("Send this message to the sequencer when continue button is clicked.")]
        public string sequencerMessage = "Resume";

        public void HideAndSendSequencerMessage()
        {
            var panel = subtitlePanel ?? GetComponentInParent<StandardUISubtitlePanel>();
            var button = GetComponent<Button>();
            subtitlePanel.HideSubtitle(DialogueManager.currentConversationState.subtitle);
            Sequencer.Message(sequencerMessage);
        }

    }

}
#endif
