﻿using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Set this OnEnter and OnExit to avoiding repeatedly comparing tags
    public bool IsPlayerColliding { get; protected set; }

    [SerializeField] new protected Collider collider;

    protected void SetPlayerIsColliding(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag) == true)
            IsPlayerColliding = true;
    }

    /// <summary>
    /// Can be called whenever we want to check whether the object is colliding with the player
    /// As it doesn't rely on trigger OnTriggerEnter and OnTriggerExit, it can be used in the rare situations where the object's
    /// position is changed without triggering OnTriggerExit (when being dropped from the player's head for instance)
    /// </summary>
    protected void SetPlayerIsColliding()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, collider.bounds.extents, Quaternion.identity);

        foreach(Collider item in colliders)
        {
            Debug.Log(item.name);   

            if (item.CompareTag(PlayerConstants.PlayerTag))
            {
                IsPlayerColliding = true;
                return;
            }
        }

        IsPlayerColliding = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        SetPlayerIsColliding(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("On trigger exit Interactable. " + other.name);
        if(other.CompareTag(PlayerConstants.PlayerTag) == true)
        {
            IsPlayerColliding = false;
        }
        Debug.Log("IsPlayerColliding = " + IsPlayerColliding);
    }
}
