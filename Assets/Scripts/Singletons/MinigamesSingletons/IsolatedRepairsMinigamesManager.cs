using System.Linq;
using UnityEngine;

public class IsolatedRepairsMinigamesManager : MonoBehaviour, IRepairsMinigamesManager
{
    public static IsolatedRepairsMinigamesManager Instance { get; private set; }

    [SerializeField]
    private RepairsMinigameContainer minigameContainer;

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
        RepairsMinigameUIManager.Instance.ResetUI();

        // Get minigame by type 
        var minigame = minigameContainer.Elements.FirstOrDefault(mg => mg != null && mg.RepairsMinigameType == minigameType);

        if (minigame == null)
            throw new System.Exception("Minigame not found with type: " + minigameType);

        parent.DestroyDirectChildren();
        var minigameObj = Instantiate(minigame.Prefab, parent);
        return minigameObj;
    }
}