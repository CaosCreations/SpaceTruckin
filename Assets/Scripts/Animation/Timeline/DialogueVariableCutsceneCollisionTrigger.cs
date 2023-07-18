using UnityEngine;

public class DialogueVariableCutsceneCollisionTrigger : CutsceneCollisionTrigger
{
    [SerializeField] private string dialogueBoolVariableName;

    protected override bool CutsceneTriggerable(Collider other)
    {
        var value = DialogueDatabaseManager.GetLuaVariableAsBool(dialogueBoolVariableName);
        Debug.Log("CutsceneCollisionTrigger dialogue variable value: " + value);
        return base.CutsceneTriggerable(other) && value;
    }
}
