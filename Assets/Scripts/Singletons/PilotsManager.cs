using UnityEngine;

public class PilotsManager : MonoBehaviour
{
    public static PilotsManager Instance;

    public Pilot[] pilots;

    void Awake()
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
                Instance.pilots[i].pilotSaveData.xp += xp;
            }
        }
    }
}
