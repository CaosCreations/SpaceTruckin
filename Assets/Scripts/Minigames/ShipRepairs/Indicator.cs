using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject greenZone;

    private Workstation workstation;
    private RepairsManager repairsManager;
    private bool isInsideGreenZone; 

    private void Start()
    {
        repairsManager = GetComponentInParent<RepairsManager>();
        workstation = GetComponentInParent<Workstation>();
        Workstation.onRotationStopped += DetermineOutcome; 
    }

    public void DetermineOutcome()
    {
        if (isInsideGreenZone)
        {
            //workstation.PlayerWins();
        }
        else
        {
            //workstation.PlayerLoses();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInsideGreenZone = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        isInsideGreenZone = false;
    }
}
