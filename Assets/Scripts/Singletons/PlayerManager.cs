using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    public PlayerData playerData;


    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (PlayerManager.Instance == null)
        {
            PlayerManager.Instance = this;
        }
        else if (PlayerManager.Instance == this)
        {
            Destroy(PlayerManager.Instance.gameObject);
            PlayerManager.Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
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
