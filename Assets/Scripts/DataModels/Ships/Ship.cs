﻿using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public class Ship : ScriptableObject
{
    [Header("Set In Editor")]
    public int id;
    public string shipName;

    public float maxHullIntegrity;
    public int maxFuel;

    public GameObject shipPrefab;
    public Sprite shipAvatar; 
    public Pilot pilot;

    [Header("Set at Runtime")]
    public bool isOwned;
    public bool isLaunched;
    public HangarNode hangarNode;
    public Mission currentMission;

    public int currentFuel;
    public float currenthullIntegrity;

    public float GetHullPercent()
    {
        return currenthullIntegrity / maxHullIntegrity;
    }

    public float GetFuelPercent()
    {
        return (float) currentFuel / maxFuel;
    }
}
