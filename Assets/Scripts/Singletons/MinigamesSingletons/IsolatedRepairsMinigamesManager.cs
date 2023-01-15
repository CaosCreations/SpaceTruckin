using System.Linq;
using UnityEngine;

public class IsolatedRepairsMinigamesManager : MonoBehaviour, IRepairsMinigamesManager
{
    public static IsolatedRepairsMinigamesManager Instance { get; private set; }

    [SerializeField]
    private RepairsMinigameContainer minigameContainer;

    private RepairsMinigameBehaviour[] repairsMinigameBehaviours;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        repairsMinigameBehaviours = FindObjectsOfType<RepairsMinigameBehaviour>();

        if (repairsMinigameBehaviours == null)
            Debug.LogError("No repairs minigames found");
    }

    public GameObject InitMinigame(RepairsMinigameType minigameType, Transform parent)
    {
        SetMinigamesActive(false);

        var minigameToActivate = repairsMinigameBehaviours.FirstOrDefault(mg => mg != null && mg.MinigameType == minigameType);
        minigameToActivate.SetActive(true);
        return default;
        //RepairsMinigameUIManager.Instance.ResetUI();

        // Get minigame by type 
        var minigame = minigameContainer.Elements.FirstOrDefault(mg => mg != null && mg.RepairsMinigameType == minigameType);

        if (minigame == null)
            throw new System.Exception("Minigame not found with type: " + minigameType);

        parent.DestroyDirectChildren();
        var minigameObj = Instantiate(minigame.Prefab, parent);
        return minigameObj;
    }

    private void SetMinigamesActive(bool isActive)
    {
        foreach (var behaviour in repairsMinigameBehaviours)
        {
            if (behaviour == null)
                continue;

            behaviour.gameObject.SetActive(isActive);
        }
    }
}