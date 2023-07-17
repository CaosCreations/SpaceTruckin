using System;
using UnityEngine;

public class CutsceneCollisionTrigger : MonoBehaviour
{
    [SerializeField] private Cutscene cutscene;

    private BoxCollider boxCollider;

    protected virtual bool CutsceneTriggerable(Collider other)
    {
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
            Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size);
        }
    }
}