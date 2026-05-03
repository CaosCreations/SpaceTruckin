#if UNITY_2021_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Manages a subtitle panel for UIToolkitDialogueUI.
    /// </summary>
    [Serializable]
    public class UIToolkitSubtitleElements : AbstractUISubtitleControls
    {

        [Tooltip("Container panel for subtitle.")]
        [SerializeField] private string subtitlePanelName;
        [Tooltip("Subtitle text.")]
        [SerializeField] private string subtitleLabelName;
        [Tooltip("Optional speaker portrait name.")]
        [SerializeField] private string portraitLabelName;
        [Tooltip("Optional speaker portrait image.")]
        [SerializeField] private string portraitImageName;
        [Tooltip("Continue button to advance conversation (if mode requires continue button click).")]
        [SerializeField] private string continueButtonName;
        [Tooltip("If typewriter is still typing, continue button fast forwards typewriter instead of advancing conversation.")]
        [SerializeField] private bool continueButtonFastForwardTypewriter;
        [Tooltip("Specifies when panel should be visible/hidden.")]
        [SerializeField] private UIVisibility visibility;

        public bool IsSamePanel(UIToolkitSubtitleElements panel) => panel.subtitlePanelName == this.subtitlePanelName;
        public string SubtitlePanelName => subtitlePanelName;
        public UIVisibility Visibility => visibility;

        protected UIDocument Document { get; set; }
        protected VisualElement SubtitlePanel => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, subtitlePanelName);
        protected VisualElement PortraitImage => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, portraitImageName);
        protected Button ContinueButton => UIToolkitDialogueUI.GetVisualElement<Button>(Document, continueButtonName);

        protected TextElement subtitleLabel = null;
        protected TextElement SubtitleLabel
        {
            get
            {
                if (subtitleLabel == null) subtitleLabel = new TextElement(Document, subtitleLabelName);
                return subtitleLabel;
            }
        }

        protected TextElement portraitLabel = null;
        protected TextElement PortraitLabel
        {
            get
            {
                if (portraitLabel == null) portraitLabel = new TextElement(Document, portraitLabelName);
                return portraitLabel;
            }
        }

        public bool ShouldStayVisible => Visibility == UIVisibility.AlwaysFromStart || Visibility == UIVisibility.AlwaysOnceShown;

        public override bool hasText => !string.IsNullOrEmpty(SubtitleLabel.text);

        protected System.Action clickedContinueAction = null;

        public virtual void Initialize(UIDocument document, System.Action clickedContinueAction)
        {
            Document = document;
            if (ContinueButton != null)
            {
                this.clickedContinueAction = clickedContinueAction;
                ContinueButton.clicked += OnContinueButtonClicked;
            }
        }

        private void OnContinueButtonClicked()
        {
            if (continueButtonFastForwardTypewriter && SubtitleLabel.IsTyping)
            {
                SubtitleLabel.FastForwardTypewriterToEnd();
            }
            else
            {
                clickedContinueAction?.Invoke();
            }
        }

        public override void SetActive(bool value)
        {
            UIToolkitDialogueUI.SetDisplay(SubtitlePanel, value);
            HideContinueButton();
        }

        public virtual void OpenOnStartConversation(Sprite portraitSprite, string portraitName, DialogueActor dialogueActor)
        {
            SetActive(true);
            var actorName = portraitName;
            var actorSprite = portraitSprite;
            if (dialogueActor != null)
            {
                actorName = dialogueActor.GetActorName();
                var dialogueActorSprite = dialogueActor.GetPortraitSprite();
                if (dialogueActorSprite != null) actorSprite = dialogueActorSprite;
            }
            SetActorPortraitSprite(actorName, actorSprite);
            if (SubtitleLabel != null) SubtitleLabel.text = string.Empty;
        }

        public override void ClearSubtitle()
        {
            if (SubtitleLabel != null) SubtitleLabel.text = string.Empty;
            HideContinueButton();
        }

        public override void SetSubtitle(Subtitle subtitle)
        {
            SetActive(true);
            if (SubtitleLabel == null) Debug.LogError("SubtitleLabel is null");
            if (SubtitleLabel != null) SubtitleLabel.text = subtitle.formattedText.text;
            SetActorPortraitSprite(subtitle.speakerInfo.Name, subtitle.GetSpeakerPortrait());
        }

        public override void SetActorPortraitSprite(string actorName, Sprite sprite)
        {
            if (PortraitLabel != null) PortraitLabel.text = actorName;
            if (PortraitImage != null)
            {
                var hasSprite = sprite != null;
                UIToolkitDialogueUI.SetDisplay(PortraitImage, hasSprite);
                if (hasSprite) PortraitImage.style.backgroundImage = new StyleBackground(sprite);
            }
        }

        public override void ShowContinueButton() => UIToolkitDialogueUI.SetDisplay(ContinueButton, true, InputDeviceManager.autoFocus);
        public override void HideContinueButton() => UIToolkitDialogueUI.SetDisplay(ContinueButton, false);

    }

}
#endif
