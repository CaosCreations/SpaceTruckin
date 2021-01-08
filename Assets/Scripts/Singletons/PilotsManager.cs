using System.Linq;
using UnityEngine;

public class PilotsManager : MonoBehaviour
{   
    public static PilotsManager Instance;

    public PilotsContainer pilotsContainer;

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
        for (int i = 0; i < Instance.pilotsContainer.pilots.Length; i++)
        {
            Instance.pilotsContainer.pilots[i].id = i;
        }
    }

    public void AwardXp(int index, int xp)
    {
        for (int i = 0; i < Instance.pilotsContainer.pilots.Length; i++)
        {
            if (Instance.pilotsContainer.pilots[i] != null && index == Instance.pilotsContainer.pilots[i].id)
            {
                Instance.pilotsContainer.pilots[i].xp += xp;
            }
        }
    }

    public void HirePilot(int index)
    {
        Instance.pilotsContainer.pilots[index].isHired = true;
    }

    public Pilot[] GetHiredPilots()
    {
        return pilotsContainer.pilots.ToList().Where(p => p.isHired).ToArray();
        
    }

    public Pilot[] GetPilotsForHire()
    {
        return pilotsContainer.pilots.ToList().Where(p => !p.isHired).ToArray();
    }
}
