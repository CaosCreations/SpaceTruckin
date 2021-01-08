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

    public void HirePilot(Pilot pilot)
    {
        pilot.isHired = true;
        int index = pilotsContainer.pilots.Length;
        pilot.id = index; 
        pilotsContainer.pilots[index] = pilot;
    }

    public Pilot[] GetPilotsForHire()
    {
        var pilotsForHire = new System.Collections.Generic.List<Pilot>();
        for (int i = 0; i < pilotsContainer.pilots.Length; i++)
        {
            if (pilotsContainer.pilots[i] != null && !pilotsContainer.pilots[i].isHired)
            {
                pilotsForHire.Add(pilotsContainer.pilots[i]);
            }
        }
        return pilotsForHire.ToArray();
    }
}
