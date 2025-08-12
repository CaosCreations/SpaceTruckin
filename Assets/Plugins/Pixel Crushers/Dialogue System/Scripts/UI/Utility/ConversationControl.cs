// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Provides AutoPlay and SkipAll functionality. To add "Auto Play" and/or 
    /// "Skip All" buttons that advances the current conversation:
    /// 
    /// - Add this script to the dialogue UI.
    /// - Add Auto Play and/or Skip All buttons to your subtitle panel(s). Configure 
    /// their OnClick() events to call the dialogue UI's ConversationControl.ToggleAutoPlay 
    /// and/or ConversationControl.SkipAll methods.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class ConversationControl : MonoBehaviour // Add to dialogue UI. Connect to Skip All and Auto Play buttons.
    {
        [Tooltip("Skip all subtitles until response menu or end of conversation is reached. Set by SkipAll().")]
        public bool skipAll;

        [Tooltip("Stop SkipAll() when unread subtitle is reached. You MUST tick Dialogue Manager's Include SimStatus checkbox to use this.")]
        public bool stopSkipAllOnUnreadSubtitle = false;

        [Tooltip("Stop SkipAll() when response menu is reached.")]
        public bool stopSkipAllOnResponseMenu = true;

        [Tooltip("Stop SkipAll() when end of conversation is reached.")]
        public bool stopSkipAllOnConversationEnd;

        [Tooltip("If Skip All is enabled, don't skip last conversation line.")]
        public bool dontSkipAllOnLastConversationLine;

        [Tooltip("Use this continue button mode when AutoPlay is on.")]
        public DisplaySettings.SubtitleSettings.ContinueButtonMode autoPlayOnContinueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Never;

        [Tooltip("Use this continue button mode when AutoPlay is off.")]
        public DisplaySettings.SubtitleSettings.ContinueButtonMode autoPlayOffContinueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;

        protected AbstractDialogueUI dialogueUI;
        protected bool mustStopAtCurrentUnreadEntry = false;
        protected bool hasStarted = false;

        protected virtual void Awake()
        {
            dialogueUI =
                GetComponent<AbstractDialogueUI>() ??
                (DialogueManager.standardDialogueUI as AbstractDialogueUI) ??
                PixelCrushers.GameObjectUtility.FindFirstObjectByType<AbstractDialogueUI>();
        }

        protected virtual void Start()
        {
            if (stopSkipAllOnUnreadSubtitle)
            {
                if (!DialogueLua.includeSimStatus)
                {
                    Debug.LogWarning("Dialogue System: Dialogue Manager's Include SimStatus isn't ticked but it requires for Stop Skip All On Unread Subtitle. Enabling SimStatus.");
                    DialogueLua.includeSimStatus = true;
                }
                DialogueManager.instance.preparingConversationLine -= OnPreparingConversationLine;
                DialogueManager.instance.preparingConversationLine += OnPreparingConversationLine;
            }
            hasStarted = true;
        }

        protected virtual void OnEnable()
        {
            if (!hasStarted) return;
            DialogueManager.instance.preparingConversationLine -= OnPreparingConversationLine;
            DialogueManager.instance.preparingConversationLine += OnPreparingConversationLine;
        }

        protected virtual void OnDisable()
        {
            DialogueManager.instance.preparingConversationLine -= OnPreparingConversationLine;
        }

        /// <summary>
        /// Toggles continue button mode between Always and Never.
        /// </summary>
        public virtual void ToggleAutoPlay()
        {
            var mode = DialogueManager.displaySettings.subtitleSettings.continueButton;
            var newMode = (mode == autoPlayOnContinueButton) ? autoPlayOffContinueButton : autoPlayOnContinueButton;
            DialogueManager.displaySettings.subtitleSettings.continueButton = newMode;
            if (newMode == autoPlayOnContinueButton)
            {
                // Just started autoplay. Advance past current line:
                dialogueUI.OnContinueConversation();
            }
            else
            {
                // Just stopped autoplay. Require continue button click:
                DialogueManager.SetContinueMode(true);
                DialogueManager.displaySettings.subtitleSettings.continueButton = autoPlayOffContinueButton;
            }
        }

        /// <summary>
        /// Skips all subtitles until response menu or end of conversation is reached.
        /// </summary>
        public virtual void SkipAll()
        {
            skipAll = true;
            if (dialogueUI != null) dialogueUI.OnContinueConversation();
        }

        public virtual void StopSkipAll()
        {
            skipAll = false;
        }

        protected virtual void OnPreparingConversationLine(DialogueEntry entry)
        {
            // If we're not stopping on unread entries, we can set this false and ignore it.
            if (!stopSkipAllOnUnreadSubtitle)
            {
                mustStopAtCurrentUnreadEntry = false;
                return;
            }
            mustStopAtCurrentUnreadEntry = DialogueLua.GetSimStatus(entry) == DialogueLua.Untouched;
        }

        public virtual void OnConversationLine(Subtitle subtitle)
        {
            if (skipAll)
            {
                var shouldSkip = !dontSkipAllOnLastConversationLine ||
                    DialogueManager.currentConversationState.hasAnyResponses;
                if (shouldSkip && !mustStopAtCurrentUnreadEntry)
                {
                    subtitle.sequence = "Continue(); " + subtitle.sequence;
                }
            }
        }

        public virtual void OnConversationResponseMenu(Response[] responses)
        {
            if (skipAll)
            {
                if (stopSkipAllOnResponseMenu) skipAll = false;
                if (dialogueUI != null) dialogueUI.ShowSubtitle(DialogueManager.currentConversationState.subtitle);
            }
        }

        public virtual void OnConversationEnd(Transform actor)
        {
            if (stopSkipAllOnConversationEnd) skipAll = false;
        }

    }
}
