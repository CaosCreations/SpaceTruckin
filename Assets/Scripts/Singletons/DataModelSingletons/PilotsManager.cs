using System.Linq;
using UnityEngine;

public class PilotsManager : MonoBehaviour, IDataModelManager
{   
    public static PilotsManager Instance { get; private set; }

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
    }

    public void Init()
    {
        if (DataModelsUtils.SaveFolderExists(Pilot.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Pilot.FOLDER_NAME);
        }
    }

    public void HirePilot(Pilot pilot)
    {
        pilot.IsHired = true;
    }

    public Pilot[] GetHiredPilots()
    {
        return Instance.Pilots.Where(p => p.IsHired) as Pilot[];
        
    }

    public Pilot[] GetPilotsForHire()
    {
        return Instance.Pilots.Where(p => !p.IsHired) as Pilot[];
    }

    public void LevelUpPilots()
    {
        foreach (Pilot pilot in Instance.Pilots)
        {
            if (pilot != null && pilot.CanLevelUp())
            {
                pilot.LevelUp();
            }
        }
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
