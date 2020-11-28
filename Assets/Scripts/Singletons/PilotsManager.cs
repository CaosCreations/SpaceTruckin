using UnityEngine;

public class PilotsManager : MonoBehaviour
{
    public static PilotsManager Instance;

    private static Pilot[] pilots; 

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
        AssignUniqueIds();
        DontDestroyOnLoad(this.gameObject);
    }

    private void AssignUniqueIds()
    {
        for (int i = 0; i < pilots.Length; i++)
        {
            pilots[i].id = i; 
        }
    }

    public static void AwardXp(int index, int xp)
    {
        for (int i = 0; i < pilots.Length; i++)
        {
            if (pilots[i] != null && index == pilots[i].id)
            {
                pilots[i].xp += xp; 
            }
        }
    }
}
