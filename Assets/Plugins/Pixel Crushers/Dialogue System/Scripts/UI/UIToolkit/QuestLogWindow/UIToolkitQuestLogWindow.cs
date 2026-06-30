#if UNITY_6000_0_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    public class UIToolkitQuestLogWindow : QuestLogWindow
    {

        [Header("UI Toolkit Configuration")]
        [Header("Quest Selection")]
        [Tooltip("Main UIDocument for quest log window.")]
        [SerializeField] private UIDocument document;
        [Tooltip("Name of document's ListView container that will hold quest & group headings.")]
        [SerializeField] private string treeViewName = "TreeView";
        [Tooltip("Quest heading template for selection tree view. Must have a Label element named exactly 'Label' for quest name and a Toggle element named exactly 'Toggle' for tracking.")]
        [SerializeField] private VisualTreeAsset questHeadingTemplate;
        [Tooltip("Color for group names.")]
        [SerializeField] private Color groupNameColor = Color.white;
        [Tooltip("Colors by quest state.")]
        [SerializeField] private UIToolkitQuestStateColors headingColors;
        [Header("Quest Details")]
        [Tooltip("Name of document's details container.")]
        [SerializeField] private string detailsContainerName = "Details";
        [Tooltip("Name of label for quest heading in details panel.")]
        [SerializeField] private string questHeadingName = "QuestHeading";
        [Tooltip("Name of label for quest heading in details panel.")]
        [SerializeField] private string questDescriptionName = "QuestDescription";
        [Tooltip("Name of document's ListView container that will hold quest entries.")]
        [SerializeField] private string questEntryListViewName = "QuestEntryListView";
        [Tooltip("Quest entry template. Must have a Label element named exactly 'Label'.")]
        [SerializeField] private VisualTreeAsset questEntryTemplate;
        [Tooltip("Colors for quest entries based on their states.")]
        [SerializeField] private UIToolkitQuestStateColors entryColors;
        [Tooltip("Show quest entries that are in these states.")]
        [SerializeField] QuestState showEntriesInState = QuestState.Active | QuestState.Success | QuestState.Failure;
        [Header("Buttons")]
        [Tooltip("Name of button to show active quests.")]
        [SerializeField] private string activeButtonName = "ActiveButton";
        [Tooltip("Name of button to show completed quests.")]
        [SerializeField] private string completedButtonName = "CompletedButton";
        [Tooltip("Name of button to close window.")]
        [SerializeField] private string closeButtonName = "CloseButton";

        protected UIDocument Document => document;
        protected TreeView TreeView => UIToolkitUtility.GetVisualElement<TreeView>(Document, treeViewName);
        protected VisualTreeAsset QuestHeadingTemplate => questHeadingTemplate;
        protected Color GroupNameColor => groupNameColor;
        protected UIToolkitQuestStateColors HeadingColors => headingColors;
        //
        protected VisualElement DetailsContainer => UIToolkitUtility.GetVisualElement<VisualElement>(Document, detailsContainerName);
        protected Label QuestHeading => UIToolkitUtility.GetVisualElement<Label>(Document, questHeadingName);
        protected Label QuestDescription => UIToolkitUtility.GetVisualElement<Label>(Document, questDescriptionName);
        protected ListView QuestEntryListView => UIToolkitUtility.GetVisualElement<ListView>(Document, questEntryListViewName);
        protected VisualTreeAsset QuestEntryTemplate => questEntryTemplate;
        protected UIToolkitQuestStateColors EntryColors => entryColors;
        protected QuestState ShowEntriesInState => showEntriesInState;
        //
        protected Button ActiveButton => UIToolkitUtility.GetVisualElement<Button>(Document, activeButtonName);
        protected Button CompletedButton => UIToolkitUtility.GetVisualElement<Button>(Document, completedButtonName);
        protected Button CloseButton => UIToolkitUtility.GetVisualElement<Button>(Document, closeButtonName);

        protected List<TreeViewItemData<IQuestOrGroupHeading>> TreeViewItemDataList { get; set; } = new List<TreeViewItemData<IQuestOrGroupHeading>>();
        protected QuestInfo SelectedQuestInfo { get; set; }

        public override void Awake()
        {
            base.Awake();
            if (Document == null) Debug.LogWarning($"{name}: UI Toolkit Document is unassigned", this);
            Document.enabled = false;
        }

        public override void OpenWindow(Action openedWindowHandler)
        {
            Document.enabled = true;
            SetupButtons();
            base.OpenWindow(openedWindowHandler);
        }

        public override void CloseWindow(Action closedWindowHandler)
        {
            Document.enabled = false;
            base.CloseWindow(closedWindowHandler);
        }

        protected virtual void SetupButtons()
        {
            ActiveButton.clicked -= ClickShowActiveQuestsButton;
            ActiveButton.clicked += ClickShowActiveQuestsButton;
            CompletedButton.clicked -= ClickShowCompletedQuestsButton;
            CompletedButton.clicked += ClickShowCompletedQuestsButton;
            CloseButton.clicked -= Close;
            CloseButton.clicked += Close;
        }

        public override void ClickShowActiveQuests(object data)
        {
            base.ClickShowActiveQuests(data);
            ActiveButton.SetEnabled(false);
            CompletedButton.SetEnabled(true);
        }

        public override void ClickShowCompletedQuests(object data)
        {
            base.ClickShowCompletedQuests(data);
            ActiveButton.SetEnabled(true);
            CompletedButton.SetEnabled(false);
        }

        protected class QuestGroup
        {
            public string Group { get; set; }
            public string GroupDisplayName { get; set; }
            public List<QuestInfo> Children { get; set; }
            public QuestGroup(string group, string groupDisplayName)
            {
                Group = group;
                GroupDisplayName = groupDisplayName;
                Children = new List<QuestInfo>();
            }
        }

        protected virtual void RebuildQuestInfoTree()
        {
            // Loop through all quests, creating tree of grouped quests
            // and saving ungrouped quests to add at end:
            var dict = new Dictionary<string, QuestGroup>();
            var ungroupedQuests = new List<QuestInfo>();
            foreach (var questInfo in quests)
            {
                if (!IsQuestVisible(questInfo)) continue;
                if (string.IsNullOrEmpty(questInfo.Group))
                {
                    // No group, so add to ungrouped dictionary entry for adding to tree at end:
                    ungroupedQuests.Add(questInfo);
                }
                else
                {
                    // Add to group:
                    if (!dict.TryGetValue(questInfo.Group, out var questGroup))
                    {
                        questGroup = new QuestGroup(questInfo.Group, questInfo.GroupDisplayName);
                        dict[questInfo.Group] = questGroup;
                    }
                    questGroup.Children.Add(questInfo);
                }
            }

            // Sort by Group name: (Note: GroupDisplayName may be different)
            var list = new List<QuestGroup>(dict.Values);
            list.Sort((x, y) => x.Group.CompareTo(y.Group));

            // Add tree view item data for grouped quests:
            TreeViewItemDataList.Clear();
            int id = 0;
            foreach (var questGroup in list)
            {
                var groupHeading = new QuestGroupHeading(questGroup.Group, questGroup.GroupDisplayName);
                var children = new List<TreeViewItemData<IQuestOrGroupHeading>>();
                foreach (var child in questGroup.Children)
                {
                    var questHeading = new QuestHeading(child);
                    var childItemData = new TreeViewItemData<IQuestOrGroupHeading>(id++, questHeading);
                    children.Add(childItemData);
                }
                var groupItemData = new TreeViewItemData<IQuestOrGroupHeading>(id++, groupHeading, children);
                TreeViewItemDataList.Add(groupItemData);
            }

            // Add ungrouped quests:
            ungroupedQuests.Sort((x, y) => x.Title.CompareTo(y.Title));
            foreach (var quest in ungroupedQuests)
            {
                var questHeading = new QuestHeading(quest);
                var questItemData = new TreeViewItemData<IQuestOrGroupHeading>(id++, questHeading);
                TreeViewItemDataList.Add(questItemData);
            }
        }

        /// <summary>
        /// Returns true unless Check Visible Field is ticked and quest's Visible field is false.
        /// </summary>
        protected virtual bool IsQuestVisible(QuestInfo questInfo)
        {
            return !(checkVisibleField && (DialogueLua.GetQuestField(questInfo.Title, "Visible").asBool == false));
        }

        public override void OnQuestListUpdated()
        {
            base.OnQuestListUpdated();
            RebuildQuestInfoTree();

            InitializeTreeViewDelegates();

            // Set the data source:
            TreeView.SetRootItems(TreeViewItemDataList);

            TreeView.Rebuild();

            // Handle clicking on a quest:
            TreeView.selectedIndicesChanged -= OnSelectedIndicesChanged;
            TreeView.selectedIndicesChanged += OnSelectedIndicesChanged;

            ActiveButton.SetEnabled(!isShowingActiveQuests);
            CompletedButton.SetEnabled(isShowingActiveQuests);
            UIToolkitUtility.SetDisplay(DetailsContainer, false);
        }

        protected virtual void InitializeTreeViewDelegates()
        {
            TreeView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            TreeView.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = InstantiateQuestHeadingTemplate();

                // Instantiate a controller for the data
                var newListEntryLogic = new UIToolkitQuestLogHeadingController();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry, GroupNameColor, HeadingColors);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };

            // Set up bind function for a specific entry
            TreeView.bindItem = (visualElement, index) =>
            {
                var controller = visualElement.userData as UIToolkitQuestLogHeadingController;
                controller.SetContent(TreeView.GetItemDataForIndex<IQuestOrGroupHeading>(index));
                int depth = TreeView.viewController.GetIndentationDepthByIndex(index);
                visualElement.RemoveFromClassList("depth-0");
                visualElement.RemoveFromClassList("depth-1");
                visualElement.AddToClassList($"depth-{depth}");
            };
        }

        protected virtual TemplateContainer InstantiateQuestHeadingTemplate() => QuestHeadingTemplate.Instantiate();

        protected virtual void OnSelectedIndicesChanged(IEnumerable<int> indices)
        {
            foreach (var index in indices)
            {
                var itemData = TreeView.GetItemDataForIndex<IQuestOrGroupHeading>(index);
                if (itemData is QuestHeading questHeading)
                {
                    SelectQuest(questHeading.QuestInfo);
                }
                break;
            }
        }

        protected virtual void SelectQuest(QuestInfo questInfo)
        {
            UIToolkitUtility.SetDisplay(DetailsContainer, true);
            SelectedQuestInfo = questInfo;
            QuestHeading.text = questInfo.Heading.text;
            QuestDescription.text = questInfo.Description.text;
            RebuildSelectedQuestEntryListView();
        }


        protected virtual void RebuildSelectedQuestEntryListView()
        {
            var hasEntries = SelectedQuestInfo.Entries.Length > 0;
            UIToolkitUtility.SetDisplay(QuestEntryListView, hasEntries);
            if (!hasEntries) return;

            // Set up a make item function for a list entry
            QuestEntryListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            QuestEntryListView.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = InstantiateQuestEntryTemplate();

                // Instantiate a controller for the data
                var newListEntryLogic = new UIToolkitQuestLogEntryController();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry, EntryColors);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };

            // Set up bind function for a specific list entry
            QuestEntryListView.bindItem = (item, index) =>
            {
                (item.userData as UIToolkitQuestLogEntryController)?.SetContent(SelectedQuestInfo.Entries[index].text, SelectedQuestInfo.EntryStates[index]);
            };

            // Set the actual item's source list/array
            QuestEntryListView.itemsSource = SelectedQuestInfo.Entries;
        }

        /// <summary>
        /// Virtual method in case you want to instantiate different template(s).
        /// </summary>
        protected virtual TemplateContainer InstantiateQuestEntryTemplate() => QuestEntryTemplate.Instantiate();

    }

}
#endif
