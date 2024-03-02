using System;
using UnityEngine;

public class CutsceneCollisionTrigger : CollisionTriggerBehaviour
{
    [SerializeField] private Cutscene cutscene;

    protected override void Awake()
    {
        base.Awake();

        if (cutscene == null)
        {
            throw new NullReferenceException("Cutscene reference missing on CutsceneCollisionTrigger " + name);
        }
    }

    protected void DestroyLinkedTriggers()
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

    protected override void TriggerBehaviour()
    {
        TimelineManager.PlayCutscene(cutscene);
        DestroyLinkedTriggers();
    }
}
