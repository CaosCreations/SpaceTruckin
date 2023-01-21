﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class StationSetupManager : MonoBehaviour
{
    public static StationSetupManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            // When changing to main station scene 
            if (SceneLoadingManager.GetSceneNameByType(SceneType.MainStation) == next.name)
            {
                Debug.Log("Setting up station...");
                SetUpStation();
            }
        };
    }

    public static void SetUpStation()
    {
        PlayerManager.Instance.SetUpPlayer();
        TimelineManager.Instance.SetUp();
        TimelineManager.PlayCutscene("Opening Cutscene");
    }
}