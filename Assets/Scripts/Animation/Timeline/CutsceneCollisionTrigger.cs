using System;
using UnityEngine;

public class CutsceneCollisionTrigger : MonoBehaviour
{
    [SerializeField] private Cutscene cutscene;
    [Header("Leave all blank that aren't used in the condition")]
    [SerializeField] private Condition[] conditions;

    private BoxCollider boxCollider;

    protected virtual bool CutsceneTriggerable(Collider other)
    {
        return other.CompareTag(PlayerConstants.PlayerTag) && conditions.AreAllMet();
    }

    private void Awake()
    {
        if (cutscene == null)
            throw new NullReferenceException("Cutscene reference missing on CutsceneCollisionTrigger " + name);

        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CutsceneTriggerable(other))
        {
            Debug.Log("Player collided with CutsceneCollisionTrigger: " + name);
            TimelineManager.PlayCutscene(cutscene);
            DestroyLinkedTriggers();
            gameObject.DestroyIfExists();
        }
    }

    private void DestroyLinkedTriggers()
    {
        var triggers = FindObjectsOfType<CutsceneCollisionTrigger>();

        for (int i = 0; i < triggers.Length; i++)
        {
            var trigger = triggers[i];
            if (trigger == null || trigger == this || trigger.cutscene != cutscene)
            {
                continue;
            }
            Debug.Log("Destroying linked trigger: " + trigger.name);
            // Trigger has the same cutscene as this trigger, so it's now redundant 
            trigger.gameObject.DestroyIfExists();
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}