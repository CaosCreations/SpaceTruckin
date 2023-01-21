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
    private static readonly Dictionary<RepairsMinigameType, GameObject> prefabMap =
        new Dictionary<RepairsMinigameType, GameObject>();

    private static readonly Dictionary<RepairsMinigameType, GameObject> instanceMap =
        new Dictionary<RepairsMinigameType, GameObject>();

    private static readonly Dictionary<RepairsMinigameType, GameObject> uiPrefabMap =
    new Dictionary<RepairsMinigameType, GameObject>();

    private static readonly Dictionary<RepairsMinigameType, GameObject> uiInstanceMap =
        new Dictionary<RepairsMinigameType, GameObject>();

    private void MapPrefab(RepairsMinigameType prefabType,
        Dictionary<RepairsMinigameType, GameObject> prefabMap,
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
        MapPrefab(RepairsMinigameType.Wheel, prefabMap, wheelMinigamePrefab);
        MapPrefab(RepairsMinigameType.Wheel, uiPrefabMap, wheelMinigameUIPrefab);

        // Stack
        MapPrefab(RepairsMinigameType.Stack, prefabMap, stackMinigamePrefab);
        MapPrefab(RepairsMinigameType.Stack, uiPrefabMap, stackMinigameUIPrefab);
    }

    private void MapInstance(RepairsMinigameType prefabType,
        Dictionary<RepairsMinigameType, GameObject> instanceMap,
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
        MapInstance(RepairsMinigameType.Wheel, instanceMap, wheelMinigameInstance);
        MapInstance(RepairsMinigameType.Wheel, uiInstanceMap, wheelMinigameUIInstance);

        // Stack
        MapInstance(RepairsMinigameType.Stack, instanceMap, stackMinigameInstance);
        MapInstance(RepairsMinigameType.Stack, uiInstanceMap, stackMinigameUIInstance);
    }
    #endregion

    #region Instantiation
    public GameObject InitPrefab(RepairsMinigameType prefabType, Transform parent)
    {
        DestroyExistingInstances();

        return InitPrefab(prefabType, prefabMap, instanceMap, parent);
    }

    public GameObject InitUIPrefab(RepairsMinigameType prefabType, Transform parent)
    {
        DestroyExistingUIInstances();

        return InitPrefab(prefabType, uiPrefabMap, uiInstanceMap, parent);
    }

    private GameObject InitPrefab(RepairsMinigameType prefabType,
        Dictionary<RepairsMinigameType, GameObject> prefabMap,
        Dictionary<RepairsMinigameType, GameObject> instanceMap,
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

    private bool MinigameHasUI(RepairsMinigameType prefabType)
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
