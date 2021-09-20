using System.Collections.Generic;
using UnityEngine;

public class MinigamePrefabManager : MonoBehaviour
{
    public static MinigamePrefabManager Instance { get; private set; }

    #region GameObjects
    // Gameplay 
    [SerializeField] private GameObject wheelMinigamePrefab;
    [SerializeField] private GameObject stackMinigamePrefab;

    private GameObject wheelMinigameInstance;
    private GameObject stackMinigameInstance;

    // UI  
    [SerializeField] private GameObject wheelMinigameUIPrefab;
    [SerializeField] private GameObject stackMinigameUIPrefab;

    private GameObject wheelMinigameUIInstance;
    private GameObject stackMinigameUIInstance;

    #endregion

    #region Mapping
    private static readonly Dictionary<RepairsMinigame, GameObject> prefabMap =
        new Dictionary<RepairsMinigame, GameObject>();

    private static readonly Dictionary<RepairsMinigame, GameObject> instanceMap =
        new Dictionary<RepairsMinigame, GameObject>();

    private static readonly Dictionary<RepairsMinigame, GameObject> uiPrefabMap =
        new Dictionary<RepairsMinigame, GameObject>();

    private static readonly Dictionary<RepairsMinigame, GameObject> uiInstanceMap =
        new Dictionary<RepairsMinigame, GameObject>();

    private void MapPrefab(RepairsMinigame prefabType,
        Dictionary<RepairsMinigame, GameObject> prefabMap,
        GameObject prefab)
    {
        if (prefab && !prefabMap.ContainsKey(prefabType))
        {
            prefabMap.Add(prefabType, prefab);
        }
        else
        {
            Debug.LogError($"{nameof(prefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference.");
        }
    }

    private void MapPrefabs()
    {
        // Wheel
        MapPrefab(RepairsMinigame.Wheel, prefabMap, wheelMinigamePrefab);
        MapPrefab(RepairsMinigame.Wheel, uiPrefabMap, wheelMinigameUIPrefab);

        // Stack
        MapPrefab(RepairsMinigame.Stack, prefabMap, stackMinigamePrefab);
        MapPrefab(RepairsMinigame.Stack, uiPrefabMap, stackMinigameUIPrefab);
    }

    private void MapInstance(RepairsMinigame prefabType,
        Dictionary<RepairsMinigame, GameObject> instanceMap,
        GameObject instance)
    {
        if (!instanceMap.ContainsKey(prefabType))
        {
            instanceMap.Add(prefabType, instance);
        }
        else
        {
            Debug.LogWarning($"{nameof(instance)} on {nameof(MinigamePrefabManager)} is already mapped.");
        }
    }

    private void MapInstances()
    {
        // Wheel
        MapInstance(RepairsMinigame.Wheel, instanceMap, wheelMinigameInstance);
        MapInstance(RepairsMinigame.Wheel, uiInstanceMap, wheelMinigameUIInstance);

        // Stack
        MapInstance(RepairsMinigame.Stack, instanceMap, stackMinigameInstance);
        MapInstance(RepairsMinigame.Stack, uiInstanceMap, stackMinigameUIInstance);
    }
    #endregion

    #region Instantiation
    public GameObject InitPrefab(RepairsMinigame prefabType, Transform parent)
    {
        DestroyExistingInstances();

        return InitPrefab(prefabType, prefabMap, instanceMap, parent);
    }

    public GameObject InitUIPrefab(RepairsMinigame prefabType, Transform parent)
    {
        DestroyExistingUIInstances();

        return InitPrefab(prefabType, uiPrefabMap, uiInstanceMap, parent);
    }

    private GameObject InitPrefab(RepairsMinigame prefabType,
        Dictionary<RepairsMinigame, GameObject> prefabMap,
        Dictionary<RepairsMinigame, GameObject> instanceMap,
        Transform parent)
    {
        instanceMap[prefabType] = Instantiate(prefabMap[prefabType], parent);
        return instanceMap[prefabType];
    }

    public static void DestroyPrefabs()
    {
        DestroyExistingInstances();
    }

    private static void DestroyExistingInstances()
    {
        foreach (var mapping in instanceMap)
        {
            mapping.Value.DestroyIfExists();
        }
    }

    private void DestroyExistingUIInstances()
    {
        foreach (var mapping in uiInstanceMap)
        {
            mapping.Value.DestroyIfExists();
        }
    }

    private bool MinigameHasUI(RepairsMinigame prefabType)
    {
        return uiInstanceMap.ContainsKey(prefabType);
    }
    #endregion

    private void Awake()
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

    private void Start()
    {
        MapPrefabs();
        MapInstances();
    }
}
