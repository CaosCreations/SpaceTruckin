#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Controls a single quest entry in a quest in the quest tracker.
    /// The visual asset must have a Label named 'Label'.
    /// </summary>
    public class UIToolkitQuestTrackerEntryController
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

        public virtual void SetContent(string questName, int entryNumber)
        {
            QuestEntry.text = QuestLog.GetQuestEntry(questName, entryNumber);
            var entryState = QuestLog.GetQuestEntryState(questName, entryNumber);
            QuestEntry.style.color = EntryColors.GetColor(entryState);
        }

    }

}
#endif
