using System.Collections.Generic;
using UnityEngine;

public enum MinigamePrefab
{
    Wheel, Stack
}

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
    private readonly Dictionary<MinigamePrefab, GameObject> prefabMap =
        new Dictionary<MinigamePrefab, GameObject>();

    private readonly Dictionary<MinigamePrefab, GameObject> instanceMap =
        new Dictionary<MinigamePrefab, GameObject>();

    private readonly Dictionary<MinigamePrefab, GameObject> uiPrefabMap =
    new Dictionary<MinigamePrefab, GameObject>();

    private readonly Dictionary<MinigamePrefab, GameObject> uiInstanceMap =
        new Dictionary<MinigamePrefab, GameObject>();

    private void MapPrefab(MinigamePrefab prefabType,
        Dictionary<MinigamePrefab, GameObject> prefabMap,
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
        MapPrefab(MinigamePrefab.Wheel, prefabMap, wheelMinigamePrefab);
        MapPrefab(MinigamePrefab.Wheel, uiPrefabMap, wheelMinigameUIPrefab);

        // Stack
        MapPrefab(MinigamePrefab.Stack, prefabMap, stackMinigamePrefab);
        MapPrefab(MinigamePrefab.Stack, uiPrefabMap, stackMinigameUIPrefab);
    }

    private void MapInstance(MinigamePrefab prefabType,
        Dictionary<MinigamePrefab, GameObject> instanceMap,
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
        MapInstance(MinigamePrefab.Wheel, instanceMap, wheelMinigameInstance);
        MapInstance(MinigamePrefab.Wheel, uiInstanceMap, wheelMinigameUIInstance);

        // Stack
        MapInstance(MinigamePrefab.Stack, instanceMap, stackMinigameInstance);
        MapInstance(MinigamePrefab.Stack, uiInstanceMap, stackMinigameUIInstance);
    }
    #endregion

    #region Instantiation
    public GameObject InitPrefab(MinigamePrefab prefabType, Transform parent)
    {
        DestroyExistingInstances();

        return InitPrefab(prefabType, prefabMap, instanceMap, parent);
    }

    public GameObject InitUIPrefab(MinigamePrefab prefabType, Transform parent)
    {
        DestroyExistingUIInstances();

        return InitPrefab(prefabType, uiPrefabMap, uiInstanceMap, parent);
    }

    private GameObject InitPrefab(MinigamePrefab prefabType,
        Dictionary<MinigamePrefab, GameObject> prefabMap,
        Dictionary<MinigamePrefab, GameObject> instanceMap,
        Transform parent)
    {
        instanceMap[prefabType] = Instantiate(prefabMap[prefabType], parent);
        return instanceMap[prefabType];
    }

    private void DestroyExistingInstances()
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

    private bool MinigameHasUI(MinigamePrefab prefabType)
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
