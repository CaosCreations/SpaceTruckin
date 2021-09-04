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
    [SerializeField] private GameObject wheelMinigamePrefab;
    [SerializeField] private GameObject stackMinigamePrefab;

    private GameObject wheelMinigameInstance;
    private GameObject stackMinigameInstance;
    #endregion

    #region Mapping
    private readonly Dictionary<MinigamePrefab, GameObject> PrefabMap =
        new Dictionary<MinigamePrefab, GameObject>();

    private readonly Dictionary<MinigamePrefab, GameObject> InstanceMap =
        new Dictionary<MinigamePrefab, GameObject>();

    private void MapPrefabs()
    {
        if (wheelMinigamePrefab && !PrefabMap.ContainsKey(MinigamePrefab.Wheel))
        {
            PrefabMap.Add(MinigamePrefab.Wheel, wheelMinigamePrefab);
        }
        else
        {
            Debug.LogError($"{nameof(wheelMinigamePrefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference.");
        }

        if (stackMinigamePrefab && !PrefabMap.ContainsKey(MinigamePrefab.Stack))
        {
            PrefabMap.Add(MinigamePrefab.Stack, stackMinigamePrefab);
        }
        else
        {
            Debug.LogError($"{nameof(stackMinigamePrefab)} on {nameof(MinigamePrefabManager)} is null. Attach the reference");
        }
    }

    private void MapInstances()
    {
        if (!PrefabMap.ContainsKey(MinigamePrefab.Wheel))
        {
            InstanceMap.Add(MinigamePrefab.Wheel, wheelMinigameInstance);
        }
        else
        {
            Debug.LogWarning($"{nameof(wheelMinigameInstance)} on {nameof(MinigamePrefabManager)} is already mapped.");
        }

        if (!InstanceMap.ContainsKey(MinigamePrefab.Stack))
        {
            InstanceMap.Add(MinigamePrefab.Stack, stackMinigameInstance);
        }
        else
        {
            Debug.LogError($"{nameof(stackMinigameInstance)} on {nameof(MinigamePrefabManager)} is already mapped.");
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

    public GameObject InitPrefab(MinigamePrefab minigamePrefab, Transform parent)
    {
        DestroyExistingInstances();

        InstanceMap[minigamePrefab] = Instantiate(PrefabMap[minigamePrefab], parent);
        return InstanceMap[minigamePrefab];
    }

    private void DestroyExistingInstances()
    {
        //wheelMinigameInstance.DestroyIfExists();
        //stackMinigameInstance.DestroyIfExists();

        foreach (var mapping in InstanceMap)
        {
            mapping.Value.DestroyIfExists();
        }
    }
}
