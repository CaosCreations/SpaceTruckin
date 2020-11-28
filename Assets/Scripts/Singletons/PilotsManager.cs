using UnityEngine;

public class PilotsManager : MonoBehaviour
{
    public static PilotsManager Instance;

    public Pilot[] pilots;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        AssignUniqueIds();
    }

    private void AssignUniqueIds()
    {
        for (int i = 0; i < Instance.pilots.Length; i++)
        {
            Instance.pilots[i].id = i;
        }
    }

    public static void AwardXp(int index, int xp)
    {
        for (int i = 0; i < Instance.pilots.Length; i++)
        {
            if (Instance.pilots[i] != null && index == Instance.pilots[i].id)
            {
                Instance.pilots[i].xp += xp;
            }
        }
    }
}
