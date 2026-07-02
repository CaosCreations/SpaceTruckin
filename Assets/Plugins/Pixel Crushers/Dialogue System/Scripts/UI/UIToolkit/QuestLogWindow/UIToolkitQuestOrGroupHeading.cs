#if UNITY_6000_0_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Base type for quest log window tree view data.
    /// </summary>
    public interface IQuestOrGroupHeading
    {
    }

    /// <summary>
    /// Subclass for an individual quest.
    /// </summary>
    public class QuestHeading : IQuestOrGroupHeading
    {
        public QuestLogWindow.QuestInfo QuestInfo { get; set; }

        public QuestHeading(QuestLogWindow.QuestInfo questInfo) => QuestInfo = questInfo;
    }

    /// <summary>
    /// Subclass for a quest group, including its child quests.
    /// </summary>
    public class QuestGroupHeading : IQuestOrGroupHeading
    {
        public string Group { get; set; }
        public string GroupDisplayName { get; set; }
        public List<QuestLogWindow.QuestInfo> QuestInfos { get; set; }

        public QuestGroupHeading(string group, string groupDisplayName)
        {
            Group = group;
            GroupDisplayName = groupDisplayName;
        }
    }

}
#endif
