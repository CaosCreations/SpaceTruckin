using System.Collections.Generic;
using UnityEngine;

public enum MinigamePrefab
{
    Wheel, Stack
}

public class MinigamePrefabManager : MonoBehaviour
{
    public static MinigamePrefabManager Instance { get; private set; }

    #region Prefab GameObjects
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

    private void MapPrefabs()
    {
        if (wheelMinigamePrefab && !prefabMap.ContainsKey(MinigamePrefab.Wheel))
        {
            prefabMap.Add(MinigamePrefab.Wheel, wheelMinigamePrefab);
        }
        else
        {
            Debug.LogError($"{nameof(wheelMinigamePrefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference.");
        }

        if (wheelMinigameUIPrefab && !prefabMap.ContainsKey(MinigamePrefab.Wheel))
        {
            uiPrefabMap.Add(MinigamePrefab.Wheel, wheelMinigameUIPrefab);
        }
        else
        {
            Debug.LogError($"{nameof(wheelMinigameUIPrefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference.");
        }

        if (stackMinigamePrefab && !prefabMap.ContainsKey(MinigamePrefab.Stack))
        {
            prefabMap.Add(MinigamePrefab.Stack, stackMinigamePrefab);
        }
        else
        {
            Debug.LogError($"{nameof(stackMinigamePrefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference");
        }

        if (wheelMinigameUIPrefab && !prefabMap.ContainsKey(MinigamePrefab.Stack))
        {
            uiPrefabMap.Add(MinigamePrefab.Stack, wheelMinigameUIPrefab);
        }
        else
        {
            Debug.LogError($"{nameof(stackMinigameUIPrefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference.");
        }
    }

    private void MapInstances()
    {
        if (!instanceMap.ContainsKey(MinigamePrefab.Wheel))
        {
            instanceMap.Add(MinigamePrefab.Wheel, wheelMinigameInstance);
        }
        else
        {
            Debug.LogWarning($"{nameof(wheelMinigameInstance)} on {nameof(MinigamePrefabManager)} is already mapped.");
        }

        if (!uiInstanceMap.ContainsKey(MinigamePrefab.Wheel))
        {
            instanceMap.Add(MinigamePrefab.Wheel, wheelMinigameUIInstance);
        }
        else
        {
            Debug.LogWarning($"{nameof(wheelMinigameUIInstance)} on {nameof(MinigamePrefabManager)} is already mapped.");
        }

        if (!instanceMap.ContainsKey(MinigamePrefab.Stack))
        {
            instanceMap.Add(MinigamePrefab.Stack, stackMinigameInstance);
        }
        else
        {
            Debug.LogError($"{nameof(stackMinigameInstance)} on {nameof(MinigamePrefabManager)} is already mapped.");
        }

        if (!uiInstanceMap.ContainsKey(MinigamePrefab.Stack))
        {
            instanceMap.Add(MinigamePrefab.Stack, stackMinigameUIInstance);
        }
        else
        {
            Debug.LogWarning($"{nameof(stackMinigameUIInstance)} on {nameof(MinigamePrefabManager)} is already mapped.");
        }
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

    #region Prefab Instantiation
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
}
