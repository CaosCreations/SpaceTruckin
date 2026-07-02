#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Controls a single quest's info in the quest tracker.
    /// The visual asset must have a Label named 'Label' and
    /// a ListView named 'ListView'.
    /// </summary>
    public class UIToolkitQuestTrackerHeadingController
    {

        protected Label QuestHeading { get; set; }
        protected UIToolkitQuestStateColors HeadingColors { get; set; }
        protected ListView ListView { get; set; }
        protected VisualTreeAsset QuestEntryTemplate { get; set; }
        protected UIToolkitQuestStateColors EntryColors { get; set; }
        protected QuestState ShowEntriesInState { get; set; }

        protected List<int> VisibleEntries { get; set; } = new List<int>();

        // This function retrieves references to UI Elements and record info
        // needed by SetContent().
        public virtual void SetVisualElement(VisualElement visualElement, UIToolkitQuestStateColors headingColors, 
            VisualTreeAsset questEntryTemplate, UIToolkitQuestStateColors entryColors, QuestState showEntriesInState)
        {
            QuestHeading = visualElement.Q<Label>("Label");
            HeadingColors = headingColors;
            ListView = visualElement.Q<ListView>("ListView");
            QuestEntryTemplate = questEntryTemplate;
            EntryColors = entryColors;
            ShowEntriesInState = showEntriesInState;
        }

        // This function receives the quest that it's supposed to display. Since
        // the elements list in a ListView are pooled and reused, it's necessary to 
        // have a Set function to change which quest's data to display.
        public virtual void SetContent(string questName)
        {
            SetHeading(questName, HeadingColors);
            SetEntries(questName, EntryColors);
        }

        protected virtual void SetHeading(string questName, UIToolkitQuestStateColors headingColors)
        { 
            QuestHeading.text = QuestLog.GetQuestTitle(questName);
            var questState = QuestLog.GetQuestState(questName);
            QuestHeading.style.color = headingColors.GetColor(questState);
        }

        protected virtual void SetEntries(string questName, UIToolkitQuestStateColors entryColors)
        {
            RecordVisibleEntries(questName);
            var hasEntries = VisibleEntries.Count > 0;
            UIToolkitUtility.SetDisplay(ListView, hasEntries);
            if (!hasEntries) return;

            // Set up a make item function for a list entry
            ListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            ListView.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = InstantiateQuestEntryTemplate();

                // Instantiate a controller for the data
                var newListEntryLogic = new UIToolkitQuestTrackerEntryController();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry, EntryColors);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };

            // Set up bind function for a specific list entry
            ListView.bindItem = (item, index) =>
            {
                (item.userData as UIToolkitQuestTrackerEntryController)?.SetContent(questName, VisibleEntries[index]);
            };

            // Set the actual item's source list/array
            ListView.itemsSource = VisibleEntries;
        }

        protected virtual void RecordVisibleEntries(string questName)
        {
            VisibleEntries.Clear();
            var entryCount = QuestLog.GetQuestEntryCount(questName);
            for (int i = 1; i <= entryCount; i++)
            {
                var entryState = QuestLog.GetQuestEntryState(questName, i);
                if ((entryState & ShowEntriesInState) != 0)
                {
                    VisibleEntries.Add(i);
                }
            }
        }

        /// <summary>
        /// Virtual method in case you want to instantiate different template(s).
        /// </summary>
        protected virtual TemplateContainer InstantiateQuestEntryTemplate() => QuestEntryTemplate.Instantiate();

    }

}
#endif
