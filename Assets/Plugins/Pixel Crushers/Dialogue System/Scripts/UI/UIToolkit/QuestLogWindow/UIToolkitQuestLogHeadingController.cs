#if UNITY_6000_0_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Controls a quest heading in the quest log window's selection tree view.
    /// Must have a Label named exactly 'Label' and a Toggle named 'Toggle'.
    /// Used for both quests and group headings. The toggle is hidden for group
    /// headings and for quests that aren't trackable.
    /// </summary>
    public class UIToolkitQuestLogHeadingController
    {

        protected Label QuestHeading { get; set; }
        protected UnityEngine.UIElements.Toggle TrackToggle { get; set; }
        protected Color GroupNameColor { get; set; }
        protected UIToolkitQuestStateColors HeadingColors { get; set; }
        protected QuestLogWindow.QuestInfo QuestInfo { get; set; }

        public virtual void SetVisualElement(VisualElement visualElement, Color groupNameColor, UIToolkitQuestStateColors headingColors)
        {
            QuestHeading = visualElement.Q<Label>("Label");
            TrackToggle = visualElement.Q<UnityEngine.UIElements.Toggle>("Toggle");
            GroupNameColor = groupNameColor;
            HeadingColors = headingColors;
            TrackToggle.RegisterCallback<ChangeEvent<bool>>(OnTrackToggleChanged);
        }

        public virtual void SetContent(IQuestOrGroupHeading questOrGroupHeading)
        {
            if (questOrGroupHeading is QuestGroupHeading questGroupHeading)
            {
                SetGroup(questGroupHeading.GroupDisplayName);
                QuestInfo = null;
            }
            else if (questOrGroupHeading is QuestHeading questHeading)
            {
                SetHeading(questHeading.QuestInfo, HeadingColors);
                QuestInfo = questHeading.QuestInfo;
            }
        }

        protected virtual void SetGroup(string groupDisplayName)
        {
            QuestHeading.text = groupDisplayName;
            QuestHeading.style.color = GroupNameColor;
            UIToolkitUtility.SetDisplay(TrackToggle, false);
        }

        protected virtual void SetHeading(QuestLogWindow.QuestInfo questInfo, UIToolkitQuestStateColors headingColors)
        { 
            QuestHeading.text = questInfo.Heading.text;
            var questState = QuestLog.GetQuestState(questInfo.Title);
            QuestHeading.style.color = headingColors.GetColor(questState);
            UIToolkitUtility.SetDisplay(TrackToggle, questInfo.Trackable);
            if (questInfo.Trackable) TrackToggle.SetValueWithoutNotify(questInfo.Track);
        }

        private void OnTrackToggleChanged(ChangeEvent<bool> evt)
        {
            QuestLog.SetQuestTracking(QuestInfo.Title, !QuestLog.IsQuestTrackingEnabled(QuestInfo.Title));
            DialogueManager.SendUpdateTracker();
        }

    }

}
#endif
