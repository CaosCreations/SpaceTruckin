#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Manages a response menu for UIToolkitDialogueUI.
    /// </summary>
    [Serializable]
    public class UIToolkitResponseMenuElements : AbstractUIResponseMenuControls
    {
        // [TODO] Options to consider: Autonumber

        [Tooltip("Container panel for response menu.")]
        [SerializeField] private string responseMenuPanelName;
        [Tooltip("Progress bar for optional timer. Value range should be 0-1.")]
        [SerializeField] private string timerProgressBarName;
        [Tooltip("Optional player portrait name.")]
        [SerializeField] private string portraitLabelName;
        [Tooltip("Optional player portrait image.")]
        [SerializeField] private string portraitImageName;
        [Tooltip("List of all available response buttons. The dialogue UI will use these to fill out the menu.")]
        [SerializeField] private List<string> responseButtonNames;

        protected UIDocument Document { get; set; }
        public override AbstractUISubtitleControls subtitleReminderControls => null;
        protected VisualElement ResponseMenuPanel => UIToolkitUtility.GetVisualElement<VisualElement>(Document, responseMenuPanelName);
        protected ProgressBar TimerProgressBar => UIToolkitUtility.GetVisualElement<ProgressBar>(Document, timerProgressBarName);
        protected VisualElement PortraitImage => UIToolkitUtility.GetVisualElement<VisualElement>(Document, portraitImageName);
        protected virtual Button GetResponseButton(int index) => UIToolkitUtility.GetVisualElement<Button>(Document, responseButtonNames[index]);

        protected TextElement portraitLabel = null;
        protected TextElement PortraitLabel
        {
            get
            {
                if (portraitLabel == null) portraitLabel = new TextElement(Document, portraitLabelName);
                return portraitLabel;
            }
        }

        protected float TimerSecondsMax { get; set; }
        protected float TimerSecondsLeft { get;set; }
        protected System.Action<object> ClickedResponseAction { get; set; }

        protected Dictionary<int, Response> ResponsesByButtonIndex = new Dictionary<int, Response>();

        public virtual void Initialize(UIDocument document, System.Action<object> clickedResponseAction)
        {
            Document = document;
            ClickedResponseAction = clickedResponseAction;
            UIToolkitUtility.SetDisplay(ResponseMenuPanel, false);
            for (int i = 0; i < responseButtonNames.Count; i++)
            {
                var index = i;
                GetResponseButton(i).clicked += () => OnClickResponse(index);
            }
        }

        public virtual void DoUpdate()
        {
            UpdateTimer();
        }

        public override void SetActive(bool value)
        {
            UIToolkitUtility.SetDisplay(ResponseMenuPanel, value);
            UIToolkitUtility.SetDisplay(TimerProgressBar, false);
        }

        public override void SetPCPortrait(Sprite sprite, string portraitName)
        {
            if (PortraitLabel != null) PortraitLabel.text = portraitName;
            if (PortraitImage != null)
            {
                var hasSprite = sprite != null;
                UIToolkitUtility.SetDisplay(PortraitImage, hasSprite);
                if (hasSprite) PortraitImage.style.backgroundImage = new StyleBackground(sprite);
            }
        }

        protected override void ClearResponseButtons()
        {
            ResponsesByButtonIndex.Clear();
            for (int i = 0; i < responseButtonNames.Count; i++)
            {
                UIToolkitUtility.SetDisplay(GetResponseButton(i), false);
            }
        }

        public override void ShowResponses(Subtitle subtitle, Response[] responses, Transform target)
        {
            if ((responses != null) && (responses.Length > 0))
            {
                ClearResponseButtons();
                SetResponseButtons(responses, target);
                Show();
            }
            else
            {
                Hide();
            }
        }

        protected override void SetResponseButtons(Response[] responses, Transform target)
        {
            var maxResponses = Mathf.Min(responses.Length, responseButtonNames.Count);
            int numUnusedButtons = responseButtonNames.Count - maxResponses;

            // Fill in buttons using specified positions & alignment:
            var needToFocusAButton = InputDeviceManager.autoFocus;
            for (int i = 0; i < responses.Length; i++)
            {
                var response = responses[i];
                var index = (response.formattedText.position != FormattedText.NoAssignedPosition)
                    ? response.formattedText.position
                    : (buttonAlignment == ResponseButtonAlignment.ToFirst)
                        ? i
                        : numUnusedButtons + i;

                ResponsesByButtonIndex[index] = response;
                var button = GetResponseButton(index);
                if (button == null) continue;
                button.text = response.formattedText.text;
                button.SetEnabled(response.enabled);
                var focusThisButton = false;
                if (needToFocusAButton && response.enabled)
                {
                    focusThisButton = true;
                    needToFocusAButton = false;
                }
                UIToolkitUtility.SetDisplay(button, true, focusThisButton);
            }

            // If specified, show unused buttons with no text:
            if (showUnusedButtons)
            {
                var firstUnusedIndex = (buttonAlignment == ResponseButtonAlignment.ToFirst) ? maxResponses : 0;
                for (int i = firstUnusedIndex; i < (firstUnusedIndex + numUnusedButtons); i++)
                {
                    var button = GetResponseButton(i);
                    if (button == null) continue;
                    button.text = string.Empty;
                    UIToolkitUtility.SetDisplay(button, true);
                }
            }
        }

        protected virtual void OnClickResponse(int index)
        {
            if (ResponsesByButtonIndex.TryGetValue(index, out var response))
            {
                Hide();
                ClickedResponseAction(response);
            }
        }

        public override void StartTimer(float timeout)
        {
            if (TimerProgressBar == null) return;
            UIToolkitUtility.SetDisplay(TimerProgressBar, true);
            TimerSecondsLeft = TimerSecondsMax = timeout;
            TimerProgressBar.value = 1;
        }

        protected virtual void UpdateTimer()
        {
            if (TimerSecondsMax <= 0) return;
            TimerSecondsLeft -= DialogueTime.deltaTime;
            TimerProgressBar.value = Mathf.Clamp01(TimerSecondsLeft / TimerSecondsMax);
            Debug.Log($"{TimerSecondsLeft} --> {TimerProgressBar.value}");

            if (TimerSecondsLeft <= 0)
            {
                TimerSecondsMax = 0;
                OnTimedOut();
            }
        }

        private void OnTimedOut()
        {
            DialogueManager.instance.SendMessage(DialogueSystemMessages.OnConversationTimeout);
        }

    }

}
#endif
