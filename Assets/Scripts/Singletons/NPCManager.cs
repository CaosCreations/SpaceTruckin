﻿using Events;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    public static NPC[] Npcs { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Npcs = FindObjectsOfType<NPC>();
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
        SingletonManager.EventService.Add<OnMorningStartEvent>(OnMorningStartHandler);

        SetMorningPositions();
    }

    private void SetNpcPosition(NPC npc, TimeOfDay.Phase phase)
    {
        if (npc.Data == null)
        {
            //Debug.LogWarning($"{npc.gameObject.name} NPC has no {nameof(NPCData)} ScriptableObject reference assigned. Cannot set TimeOfDay position.");
            return;
        }

        var location = npc.Data.GetLocationByDate(CalendarManager.CurrentDate);
        var position = location.GetPositionByPhase(phase);

        // Set position if configured
        if (position != Vector3.zero)
        {
            Debug.Log($"Setting NPC '{npc.name}' {phase} position to '{position}'");
            npc.transform.position = position;
        }

        var stateName = location.GetStateByPhase(phase);

        // Set animation state if configured
        if (!string.IsNullOrWhiteSpace(stateName))
        {
            Debug.Log($"Playing NPC '{npc.name}' {phase} animation state '{stateName}'");
            npc.Animator.Play(stateName);
        }
    }

    private void SetMorningPositions()
    {
        foreach (var npc in Npcs)
        {
            SetNpcPosition(npc, TimeOfDay.Phase.Morning);
        }
    }

    private void SetEveningPositions()
    {
        foreach (var npc in Npcs)
        {
            SetNpcPosition(npc, TimeOfDay.Phase.Evening);
        }
    }

    private void OnMorningStartHandler()
    {
        SetMorningPositions();
    }

    private void OnEveningStartHandler()
    {
        // Change positions of NPC's when evening starts each day
        SetEveningPositions();
    }
}