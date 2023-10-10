using System;
using UnityEngine;

public class CutsceneCollisionTrigger : MonoBehaviour
{
    [SerializeField] private Cutscene cutscene;
    [Header("Leave blank if not used in the condition")]
    [SerializeField] private string dialogueBoolVariableName;
    [SerializeField] private Mission mission;

    private BoxCollider boxCollider;

    protected virtual bool CutsceneTriggerable(Collider other)
    {
        if (!string.IsNullOrWhiteSpace(dialogueBoolVariableName))
        {
            var value = DialogueDatabaseManager.GetLuaVariableAsBool(dialogueBoolVariableName);
            Debug.Log("CutsceneCollisionTrigger dialogue variable value: " + value);
            
            if (!value)
            {
                return false;
            }
        }

        if (mission != null && !mission.HasBeenCompleted)
        {
            return false;
        }
        return other.CompareTag(PlayerConstants.PlayerTag);
    }

    private void Awake()
    {
        if (cutscene == null)
            throw new NullReferenceException("Cutscene reference missing on CutsceneCollisionTrigger");

        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CutsceneTriggerable(other))
        {
            Debug.Log("Player collided with CutsceneCollisionTrigger");
            TimelineManager.PlayCutscene(cutscene);
            gameObject.DestroyIfExists();
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(boxCollider.transform.position + boxCollider.center, boxCollider.size);
        }
    }
}