#if UNITY_2021_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Top-level dialogue panel for UIToolkitDialogueUI. Manages subtitle panels and response menus.
    /// </summary>
    [Serializable]
    public class UIToolkitDialogueElements : AbstractDialogueUIControls
    {

        [SerializeField] private UIDocument document;
        [Tooltip("Name of document's root container.")]
        [SerializeField] private string rootContainerName;
        [SerializeField] private string dialoguePanelName;
        [Tooltip("Index (starting from 0) into Subtitle Panel Elements of the default NPC subtitle panel.")]
        [SerializeField] int npcSubtitlePanelIndex = 0;
        [Tooltip("Index (starting from 0) into Subtitle Panel Elements of the default PC subtitle panel.")]
        [SerializeField] int pcSubtitlePanelIndex = 1;
        [SerializeField] private List<UIToolkitSubtitleElements> subtitlePanelElements;
        [SerializeField] private UIToolkitResponseMenuElements responseMenuElements;

        public List<UIToolkitSubtitleElements> SubtitlePanelElements => subtitlePanelElements;
        public UIToolkitSubtitleElements NPCSubtitleElements => GetSubtitleElements(npcSubtitlePanelIndex);
        public UIToolkitSubtitleElements PCSubtitleElements => GetSubtitleElements(pcSubtitlePanelIndex);
        public UIToolkitSubtitleElements GetSubtitleElements(int index)
        {
            return (0 <= index && index < subtitlePanelElements.Count) ? subtitlePanelElements[index] : null;
        }

        protected UIDocument Document => document;
        protected VisualElement RootContainer => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, rootContainerName);
        protected VisualElement DialoguePanel => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, dialoguePanelName);
        public override AbstractUISubtitleControls npcSubtitleControls => NPCSubtitleElements;
        public override AbstractUISubtitleControls pcSubtitleControls => PCSubtitleElements;
        public override AbstractUIResponseMenuControls responseMenuControls => responseMenuElements;

        public void Initialize(System.Action clickedContinueAction, System.Action<object> clickedResponseAction)
        {
            responseMenuElements.Initialize(Document, clickedResponseAction);
            subtitlePanelElements.ForEach(x => x.Initialize(Document, clickedContinueAction));
        }

        public override void ShowPanel()
        {
            UIToolkitDialogueUI.SetInteractable(RootContainer, false);
            UIToolkitDialogueUI.SetDisplay(DialoguePanel, true);
        }

        public override void SetActive(bool value)
        {
            UIToolkitDialogueUI.SetInteractable(RootContainer, value);
            UIToolkitDialogueUI.SetDisplay(DialoguePanel, value);
            base.SetActive(value);
        }

    }

}
#endif
