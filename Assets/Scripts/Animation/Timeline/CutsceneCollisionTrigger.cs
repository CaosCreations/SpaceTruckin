using System;
using UnityEngine;

public class CutsceneCollisionTrigger : MonoBehaviour
{
    [SerializeField] private Cutscene cutscene;

    private void Awake()
    {
        if (cutscene == null)
            throw new NullReferenceException("Cutscene reference missing on CutsceneCollisionTrigger");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag))
        {
            Debug.Log("Player collided with CutsceneCollisionTrigger");
            TimelineManager.PlayCutscene(cutscene);
            gameObject.DestroyIfExists();
        }
    }
}