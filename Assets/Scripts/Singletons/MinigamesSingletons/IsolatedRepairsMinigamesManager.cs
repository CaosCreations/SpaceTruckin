using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        var minigame = minigameContainer.Elements
            .FirstOrDefault(mg => mg != null && mg.RepairsMinigameType == minigameType);

        if (minigame == null)
            throw new System.Exception("Minigame not found in container with type: " + minigameType);

        Debug.Log("Starting minigame with type: " + minigameType);

        SceneLoadingManager.Instance.LoadSceneAsync(minigame.Scene, loadSceneMode: LoadSceneMode.Additive);
        return default;
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