using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    public PlayerData playerData;

    [Header("Set at Runtime")]
    public bool isPaused;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if(playerData == null)
        {
            Debug.LogError("No player data found");
        }
    }

    void Update()
    {
        
    }

    public static void VendingMachine()
    {

    }
}
