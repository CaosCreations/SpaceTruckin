using System.Linq;
using UnityEngine;

public class IsolatedRepairsMinigamesManager : MonoBehaviour, IRepairsMinigamesManager
{
    public static IsolatedRepairsMinigamesManager Instance { get; private set; }

    [SerializeField]
    private RepairsMinigameContainer minigameContainer;

    [SerializeField]
    private Transform minigameParent;

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

    public GameObject InitMinigame(RepairsMinigameType minigameType, Transform parent)
    {
        var minigame = minigameContainer.Elements.FirstOrDefault(mg => mg != null && mg.RepairsMinigameType == minigameType);

        if (minigame == null)
            throw new System.Exception("Minigame not found with type: " + minigameType);

        minigameParent.DestroyDirectChildren();
        var minigameObj = Instantiate(minigame.Prefab);

        return minigameObj;
    }
}