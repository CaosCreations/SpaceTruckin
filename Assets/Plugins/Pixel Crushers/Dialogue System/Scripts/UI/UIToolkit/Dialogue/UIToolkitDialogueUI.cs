#if UNITY_2021_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Dialogue UI implementation for UI Toolkit.
    /// </summary>
    public class UIToolkitDialogueUI : AbstractDialogueUI, IDialogueUI
    {

        [SerializeField] private UIToolkitRootElements rootElements;
        [SerializeField] private UIToolkitAlertElements alertElements;
        [SerializeField] private UIToolkitDialogueElements dialogueElements;
        [SerializeField] private UIToolkitQTEElements qteElements;

        public override AbstractUIRoot uiRootControls => rootElements;
        public override AbstractDialogueUIControls dialogueControls => dialogueElements;
        public override AbstractUIQTEControls qteControls => qteElements;
        public override AbstractUIAlertControls alertControls => alertElements;

        public override void Awake()
        {
            base.Awake();
            dialogueElements.Initialize(OnContinueConversation, OnClick);
        }

        public override void Update()
        {
            base.Update();
            (dialogueElements.responseMenuControls as UIToolkitResponseMenuElements).DoUpdate();
        }

        public override void Open()
        {
            base.Open();
            OpenSubtitlePanelsOnStart();
        }

        public override void ShowSubtitle(Subtitle subtitle)
        {
            var panel = GetSubtitlePanel(subtitle);
            if (panel != null)
            {
                HideOtherApplicablePanels(panel);
                panel.ShowSubtitle(subtitle);
            }
        }

        public override void HideSubtitle(Subtitle subtitle)
        {
            var panel = GetSubtitlePanel(subtitle);
            if (panel != null && !panel.ShouldStayVisible) panel.Hide();
        }

        protected virtual UIToolkitSubtitleElements GetSubtitlePanel(int index)
        {
            return (0 <= index && index < dialogueElements.SubtitlePanelElements.Count)
                ? dialogueElements.SubtitlePanelElements[index] : null;
        }

        protected virtual UIToolkitSubtitleElements GetSubtitlePanel(Subtitle subtitle)
        {
            if (subtitle == null) return null;

            // Check for override via [panel=#] tag:
            var panel = GetSubtitlePanel(subtitle.formattedText.subtitlePanelNumber);
            if (panel != null) return panel;

            // Check for override on speaker's DialogueActor component:
            var dialogueActor = DialogueActor.GetDialogueActorComponent(subtitle.speakerInfo.transform);
            panel = GetDialogueActorSubtitlePanel(dialogueActor);
            if (panel != null) return panel;

            // Otherwise choose standard panel:
            return subtitle.speakerInfo.isNPC
                ? dialogueElements.npcSubtitleControls as UIToolkitSubtitleElements
                : dialogueElements.pcSubtitleControls as UIToolkitSubtitleElements;
        }

        protected virtual UIToolkitSubtitleElements GetDialogueActorSubtitlePanel(DialogueActor dialogueActor)
        {
            if (dialogueActor != null && dialogueActor.standardDialogueUISettings.subtitlePanelNumber != SubtitlePanelNumber.Default)
            {
                var index = PanelNumberUtility.GetSubtitlePanelIndex(dialogueActor.standardDialogueUISettings.subtitlePanelNumber);
                return GetSubtitlePanel(index);
            }
            return null;
        }

        protected virtual void OpenSubtitlePanelsOnStart()
        {
            var conversation = DialogueManager.masterDatabase.GetConversation(DialogueManager.lastConversationStarted);
            if (conversation == null) return;
            HashSet<UIToolkitSubtitleElements> checkedPanels = new HashSet<UIToolkitSubtitleElements>();
            HashSet<int> checkedActorIDs = new HashSet<int>();

            // Check main Actor & Conversant:
            var mainActorID = conversation.ActorID;
            var mainActor = DialogueManager.masterDatabase.GetActor(DialogueActor.GetActorName(DialogueManager.currentActor));
            if (mainActor != null) mainActorID = mainActor.id;
            CheckActorIDOnStartConversation(mainActorID, checkedActorIDs, checkedPanels);
            CheckActorIDOnStartConversation(conversation.ConversantID, checkedActorIDs, checkedPanels);

            // Check other actors:
            for (int i = 0; i < conversation.dialogueEntries.Count; i++)
            {
                var actorID = conversation.dialogueEntries[i].ActorID;
                CheckActorIDOnStartConversation(actorID, checkedActorIDs, checkedPanels);
            }
        }

        protected virtual void CheckActorIDOnStartConversation(int actorID, HashSet<int> checkedActorIDs, HashSet<UIToolkitSubtitleElements> checkedPanels)
        {
            if (checkedActorIDs.Contains(actorID)) return;
            checkedActorIDs.Add(actorID);
            var actor = DialogueManager.MasterDatabase.GetActor(actorID);
            if (actor == null) return;
            var actorTransform = GetActorTransform(actor.Name);
            DialogueActor dialogueActor;
            var defaultPanel = actor.IsPlayer ? dialogueElements.PCSubtitleElements : dialogueElements.NPCSubtitleElements;
            var panel = GetActorTransformPanel(actorTransform, defaultPanel, out dialogueActor);
            if (panel == null && actorTransform == null && Debug.isDebugBuild) Debug.LogWarning("Dialogue System: Can't determine what subtitle panel to use for " + actor.Name, actorTransform);
            if (panel == null || checkedPanels.Contains(panel)) return;
            checkedPanels.Add(panel);
            if (panel.Visibility == UIVisibility.AlwaysFromStart)
            {
                var actorPortrait = (dialogueActor != null && dialogueActor.GetPortraitSprite() != null) ?
                    dialogueActor.GetPortraitSprite() : actor.GetPortraitSprite();
                var actorName = CharacterInfo.GetLocalizedDisplayNameInDatabase(actor.Name);
                panel.OpenOnStartConversation(actorPortrait, actorName, dialogueActor);
            }
        }

        protected virtual Transform GetActorTransform(string actorName)
        {
            var actorTransform = CharacterInfo.GetRegisteredActorTransform(actorName);
            if (actorTransform == null)
            {
                var go = GameObject.Find(actorName);
                if (go != null) actorTransform = go.transform;
            }
            return actorTransform;
        }

        public virtual UIToolkitSubtitleElements GetActorTransformPanel(Transform speakerTransform, UIToolkitSubtitleElements defaultPanel, 
            out DialogueActor dialogueActor)
        {
            dialogueActor = null;
            if (speakerTransform == null) return defaultPanel;
            dialogueActor = DialogueActor.GetDialogueActorComponent(speakerTransform);
            if (dialogueActor != null && dialogueActor.standardDialogueUISettings.subtitlePanelNumber != SubtitlePanelNumber.Default)
            {
                var panel = GetDialogueActorSubtitlePanel(dialogueActor);
                if (panel != null) return panel;
            }
            return defaultPanel;
        }

        protected virtual void HideOtherApplicablePanels(UIToolkitSubtitleElements panel)
        {
            foreach (var otherPanel in dialogueElements.SubtitlePanelElements)
            {
                if (otherPanel.IsSamePanel(panel)) continue;
                if (otherPanel.ShouldStayVisible) continue;
                otherPanel.Hide();
            }
        }

        public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
        {
            base.ShowResponses(subtitle, responses, timeout);
        }

        #region Static Utility Methods

        public static void SetDisplay(VisualElement visualElement, bool value)
        {
            if (visualElement == null) return;
            visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public static bool IsVisible(VisualElement visualElement)
        {
            if (visualElement == null) return false;
            return visualElement.style.display != DisplayStyle.None;

        }

        public static T GetVisualElement<T>(UIDocument document, string visualElementName) where T : VisualElement
        {
            if (document == null || document.rootVisualElement == null) return null;
            return document.rootVisualElement.Q<T>(visualElementName);
        }

        public static void SetInteractable(VisualElement rootVisualElement, bool value)
        {
            if (rootVisualElement == null) return;
            rootVisualElement.pickingMode = value ? PickingMode.Position : PickingMode.Ignore;
        }

        #endregion

    }

}
#endif
