#if UNITY_6000_0_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Controls a single quest entry in the selected quest in the quest log window.
    /// The visual asset must have a Label named 'Label'.
    /// </summary>
    public class UIToolkitQuestLogEntryController
    {

        protected Label QuestEntry { get; set; }
        protected UIToolkitQuestStateColors EntryColors { get; set; }

        // This function retrieves a reference to the 
        // character name label inside the UI element.
        public virtual void SetVisualElement(VisualElement visualElement, UIToolkitQuestStateColors entryColors)
        {
            QuestEntry = visualElement.Q<Label>("Label");
            EntryColors = entryColors;
        }

        public virtual void SetContent(string entryText, QuestState entryState)
        {
            QuestEntry.text = entryText;
            QuestEntry.style.color = EntryColors.GetColor(entryState);
        }

    }

}
#endif
