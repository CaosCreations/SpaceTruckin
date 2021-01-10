using System.Linq;
using UnityEngine;

public class PilotsManager : MonoBehaviour, IDataModelManager
{   
    public static PilotsManager Instance;

    public PilotsContainer pilotsContainer;
    public Pilot[] Pilots { get => pilotsContainer.pilots; }

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
        for (int i = 0; i < Instance.Pilots.Length; i++)
        {
            Instance.Pilots[i].id = i;
        }
    }

    public void AwardXp(Ship ship, int xp)
    {
        if (ship != null && ship.Pilot != null)
        {
            ship.Pilot.Xp += xp;
        }

        //for (int i = 0; i < Instance.Pilots.Length; i++)
        //{
        //    if (Instance.Pilots[i] != null && index == Instance.Pilots[i].id)
        //    {
        //        Instance.Pilots[i].Xp += xp;
        //    }
        //}
    }

    public void HirePilot(int index)
    {
        Instance.Pilots[index].IsHired = true;
    }

    public Pilot[] GetHiredPilots()
    {
        return Instance.Pilots.ToList().Where(p => p.IsHired).ToArray();
        
    }

    public Pilot[] GetPilotsForHire()
    {
        return Instance.Pilots.ToList().Where(p => !p.IsHired).ToArray();
    }

    public void SaveData()
    {
        foreach (Pilot pilot in Instance.Pilots)
        {
            pilot.SaveData();
        }
    }

    public async void LoadDataAsync()
    {
        foreach (Pilot pilot in Instance.Pilots)
        {
            await pilot.LoadDataAsync();
        }
    }

    public void DeleteData()
    {
        Instance.DeleteData();
    }
}
