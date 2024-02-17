using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DialogueEditor
{
    [MenuItem("Space Truckin/Dialogue/Lift UI Access")]
    public static void LiftUIAccess()
    {
        UIManager.LiftAccessSettings();
    }

    [MenuItem("Space Truckin/Dialogue/Print Seen Vars")]
    public static void PrintSeenVars()
    {
        if (DialogueDatabaseManager.Instance == null)
        {
            return;
        }
        var info = DialogueDatabaseManager.GetSeenInfo();
        info.ForEach(i => Debug.Log(i));
    }

    [MenuItem("Space Truckin/Dialogue/Lori Day 1 Pre-Terminal")]
    public static void LoriDay1PreTerminal()
    {
        if (DialogueDatabaseManager.Instance == null)
        {
            return;
        }

        var variables = new Dictionary<string, bool>
        {
            { "Lori_Convo_1_seen", true },
            { "Lori_Convo_2_seen" , true },
            { "Lori_Convo_3_seen", true  },
            { "Lori_Convo_4_seen", true  },
            { "Qza_Convo_1_seen", true  },
            { "Pirates_Convo_1_seen", true },
            { "PR_Convo_1_seen", true },
            { "ULSS_Convo_1_Rajni_Third_seen", true },
        };

        DialogueDatabaseManager.Instance.UpdateDatabaseVariables(variables);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Terminal, CalendarManager.CurrentDate, false);
    }

    [MenuItem("Space Truckin/Dialogue/Day 2")]
    public static void Day2()
    {
        if (DialogueDatabaseManager.Instance == null)
        {
            return;
        }

        MissionsEditor.StartMission("Alcohol for adults", "B. Lrorllyl");
        MissionsEditor.AcceptMission("Vukra Aid");
        MissionsEditor.AcceptMission("Towels");

        var ids = new int[] { 166, 167, 171, 172, 163, 168, 205, 175, 51, 58, 45, };
        DialogueDatabaseManager.Instance.SetConversationsSeen(ids);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Terminal, new Date(1, 1, 1), true);
        LiftUIAccess();

        Bed.Sleep();
    }
}
