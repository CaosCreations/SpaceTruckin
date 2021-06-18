using PixelCrushers.DialogueSystem;
using System;
using System.Linq;
using UnityEngine;

public class DialogueActor : MonoBehaviour
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;

    private void Start()
    {
        string dateCondition = dialogueSystemTrigger.condition.luaConditions
            .FirstOrDefault(x => x.StartsWith(DialogueConstants.DateReachedFunctionName));

        Debug.Log("Date condition: " + dateCondition);
    }

    private void MoveConversationDateCondition(int daysToMoveBy = 1) 
    {
        int dateConditionIndex = Array.FindIndex(
            dialogueSystemTrigger.condition.luaConditions,
            x => x.StartsWith(DialogueConstants.DateReachedFunctionName));

        string dateCondition = dialogueSystemTrigger.condition.luaConditions[dateConditionIndex];

        if (string.IsNullOrEmpty(dateCondition))
        {
            Debug.Log("Date condition not found");
            return;
        }

        double[] dateParams = Array.ConvertAll(dateCondition.Substring(dateCondition.IndexOf('('))
                .TrimStart('(')
                .TrimEnd(')')
                .Split(','), x => double.Parse(x));

        double numberOfDays = CalendarUtils.ConvertDateToDays(dateParams) + daysToMoveBy;
        string dateString = CalendarUtils.ConvertDaysToDateString(numberOfDays);

        string newDateCondition = DialogueConstants.DateReachedFunctionName + dateString;

        dialogueSystemTrigger.condition.luaConditions[dateConditionIndex] = newDateCondition;
    }
}
