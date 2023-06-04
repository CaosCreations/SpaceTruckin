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
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);

        SetMorningPositions();
    }

    private void SetNpcPosition(NPC npc, TimeOfDay.Phase phase)
    {
        if (npc.Data == null)
        {
            Debug.LogWarning($"{npc.gameObject.name} NPC has no {nameof(NPCData)} ScriptableObject reference assigned. Cannot set TimeOfDay position.");
            return;
        }

        var location = npc.Data.GetLocationByDate(CalendarManager.CurrentDate);
        var position = location.GetPositionByPhase(phase);

        // Don't set position if it's not configured
        if (position == Vector3.zero)
            return;

        Debug.Log($"Setting NPC '{npc.name}' {phase} position to '{position}'");
        npc.transform.position = position;
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

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        SetMorningPositions();
    }

    private void OnEveningStartHandler()
    {
        // Change positions of NPC's when evening starts each day
        SetEveningPositions();
    }
}