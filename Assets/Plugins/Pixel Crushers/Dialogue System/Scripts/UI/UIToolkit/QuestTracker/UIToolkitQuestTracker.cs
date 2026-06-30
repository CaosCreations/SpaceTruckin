#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// UI Toolkit implementation of quest tracker HUD.
    /// </summary>
    public class UIToolkitQuestTracker : MonoBehaviour
    {

        [Tooltip("Record the quest tracker display toggle in this PlayerPrefs key. Leave blank to not record tracker visible/invisible state.")]
        [SerializeField] private string playerPrefsToggleKey = "QuestTracker";
        [Tooltip("Main UIDocument for quest tracker.")]
        [SerializeField] private UIDocument document;
        [Tooltip("Name of document's ListView container that will hold quest headings.")]
        [SerializeField] private string listViewName = "ListView";
        [Tooltip("Quest heading template. Must have a Label element named exactly 'Label' and ListView named 'ListView' for quest entries.")]
        [SerializeField] private VisualTreeAsset questHeadingTemplate;
        [Tooltip("Colors for quest entries based on their states.")]
        [SerializeField] private UIToolkitQuestStateColors headingColors;
        [Tooltip("Show quests that are in these states.")]
        [SerializeField] QuestState showQuestsInState = QuestState.Active;
        [Tooltip("Quest entry template. Must have a Label element named exactly 'Label'.")]
        [SerializeField] private VisualTreeAsset questEntryTemplate;
        [Tooltip("Colors for quest entries based on their states.")]
        [SerializeField] private UIToolkitQuestStateColors entryColors;
        [Tooltip("Show quest entries that are in these states.")]
        [SerializeField] QuestState showEntriesInState = QuestState.Active | QuestState.Success | QuestState.Failure;
        [Tooltip("Update tracker when this component starts.")]
        [SerializeField] private bool visibleOnStart = true;
        [Tooltip("Open Quest Log Window when selecting a tracker entry.")]
        [SerializeField] private bool openQuestLog = true;
        [SerializeField] private QuestLogWindow questLogWindow;

        protected UIDocument Document => document;
        protected ListView ListView => UIToolkitUtility.GetVisualElement<ListView>(Document, listViewName);
        protected VisualTreeAsset QuestHeadingTemplate => questHeadingTemplate;
        protected VisualTreeAsset QuestEntryTemplate => questEntryTemplate;
        protected UIToolkitQuestStateColors HeadingColors => headingColors;
        protected UIToolkitQuestStateColors EntryColors => entryColors;
        protected QuestState ShowQuestsInState => showQuestsInState;
        protected QuestState ShowEntriesInState => showEntriesInState;
        protected bool VisibleOnStart => visibleOnStart;
        protected string PlayerPrefsToggleKey => playerPrefsToggleKey;

        protected virtual bool IsVisible { get; set; } = true;
        protected List<string> QuestNames { get; set; } = new List<string>();

        protected virtual void Start()
        {
            if (Document == null) Debug.LogWarning($"{name}: UI Toolkit Document is unassigned", this);
            if (ListView == null) Debug.LogWarning($"{name}: Root Container is unassigned", this);
            if (QuestHeadingTemplate == null) Debug.LogWarning($"{name}: Quest Heading template is unassigned", this);
            RegisterForUpdateTrackerEvents();
            IsVisible = string.IsNullOrEmpty(PlayerPrefsToggleKey) || PlayerPrefs.GetInt(PlayerPrefsToggleKey, VisibleOnStart ? 1 : 0) == 1;
            if (IsVisible)
            {
                StartCoroutine(UpdateTrackerAfterFrame());
            }
            else
            {
                HideTracker();
            }
        }

        protected virtual void OnEnable() => RegisterForUpdateTrackerEvents();

        protected virtual void OnDisable() => UnregisterFromUpdateTrackerEvents();

        protected void RegisterForUpdateTrackerEvents()
        {
            if (DialogueManager.instance == null) return;
            if (GetComponentInParent<DialogueSystemController>() != null) return; // Children of Dialogue Manager automatically receive UpdateTracker; no need to register.
            DialogueManager.instance.receivedUpdateTracker -= UpdateTracker;
            DialogueManager.instance.receivedUpdateTracker += UpdateTracker;
        }

        protected void UnregisterFromUpdateTrackerEvents()
        {
            if (DialogueManager.instance == null) return;
            DialogueManager.instance.receivedUpdateTracker -= UpdateTracker;
        }

        protected virtual IEnumerator UpdateTrackerAfterFrame()
        {
            yield return null;
            ShowTracker();
        }

        /// <summary>
        /// Makes the quest tracker HUD visible and updates its content.
        /// </summary>
        public virtual void ShowTracker()
        {
            IsVisible = true;
            if (!string.IsNullOrEmpty(playerPrefsToggleKey)) PlayerPrefs.SetInt(playerPrefsToggleKey, 1);
            UIToolkitUtility.SetDisplay(ListView, true);
            UpdateTracker();
        }

        /// <summary>
        /// Hides the quest tracker HUD entirely.
        /// </summary>
        public virtual void HideTracker()
        {
            IsVisible = false;
            if (!string.IsNullOrEmpty(playerPrefsToggleKey)) PlayerPrefs.SetInt(playerPrefsToggleKey, 0);
            UIToolkitUtility.SetDisplay(ListView, false);
        }

        /// <summary>
        /// Toggles the quest tracker HUD visibility.
        /// </summary>
        public virtual void ToggleTracker()
        {
            if (IsVisible) HideTracker(); else ShowTracker();
        }

        /// <summary>
        /// The quest log window sends this message when the player toggles tracking.
        /// </summary>
        public virtual void OnQuestTrackingEnabled(string quest) => UpdateTracker();

        /// <summary>
        /// The quest log window sends this message when the player toggles tracking.
        /// </summary>
        public virtual void OnQuestTrackingDisabled(string quest) => UpdateTracker();

        /// <summary>
        /// Quests are often completed in conversations. This handles changes in quest states
        /// after conversations.
        /// </summary>
        public void OnConversationEnd(Transform actor) => UpdateTracker();

        public virtual void UpdateTracker()
        {
            if (!IsVisible) return;

            // Record tracked quests. If none, hide tracker:
            RecordTrackedQuests();
            var hasQuests = QuestNames.Count > 0;
            UIToolkitUtility.SetDisplay(ListView, hasQuests);
            if (!hasQuests) return;

            InitializeListView();

            // Set the actual item's source list/array
            ListView.itemsSource = QuestNames;

            ListView.Rebuild();
        }

        protected virtual void InitializeListView()
        {
            // Set up a make item function for a list entry
            ListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            ListView.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = InstantiateQuestHeadingTemplate();

                // Instantiate a controller for the data
                var newListEntryLogic = new UIToolkitQuestTrackerHeadingController();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry, HeadingColors, QuestEntryTemplate, EntryColors, ShowEntriesInState);

                newListEntry.RegisterCallback<ClickEvent>(OnClicked);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };

            // Set up bind function for a specific list entry
            ListView.bindItem = (item, index) =>
            {
                (item.userData as UIToolkitQuestTrackerHeadingController)?.SetContent(QuestNames[index]);
            };

            //--- UIToolkit currently intermittently invokes selectionChanged on hover, too,
            //--- so now we register for ClickEvents in the list entry instead:
            //ListView.selectionChanged += OnEntrySelected;
        }

        private void OnClicked(ClickEvent evt)
        {
            OnEntrySelected(null);
        }

        protected virtual void OnEntrySelected(IEnumerable<object> selectedEntries)
        {
            if (openQuestLog && ListView.selectedItems.Count() > 0)
            {
                if (questLogWindow == null)
                {
                    questLogWindow = GameObjectUtility.FindFirstObjectByType<QuestLogWindow>();
                    if (questLogWindow == null) return; // No quest log window in scene.
                }
                if (!questLogWindow.IsOpen)
                {
                    questLogWindow.Open();
                    ListView.ClearSelection();
                }
            }
        }

        /// <summary>
        /// Virtual method in case you want to instantiate different template(s).
        /// </summary>
        protected virtual TemplateContainer InstantiateQuestHeadingTemplate() => QuestHeadingTemplate.Instantiate();

        protected virtual void RecordTrackedQuests()
        {
            QuestNames.Clear();
            foreach (var questName in QuestLog.GetAllQuests(ShowQuestsInState))
            {
                if (QuestLog.IsQuestTrackingEnabled(questName))
                {
                    QuestNames.Add(questName);
                }
            }
        }

    }

}
#endif
