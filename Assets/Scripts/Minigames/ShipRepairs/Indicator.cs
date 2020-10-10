using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Could do with a better name 
public class Indicator : MonoBehaviour
{
    public GameObject greenZone;

    private Workstation workstation;

    private void Start()
    {
        workstation = GetComponentInParent<Workstation>();
    }

    // If indicator is colliding with the green zone, 
    // the player wins the minigame. 
    public void DetermineOutcome(bool colliding)
    {
        if (colliding)
        {
            workstation.PlayerWins();
        }
        else
        {
            workstation.PlayerLoses(); 
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        // We should only check for a collision when the player has stopped the spin.
        if (workstation.minigameStarted)
        {
            DetermineOutcome(other.gameObject == greenZone); 
        }
    }
}
