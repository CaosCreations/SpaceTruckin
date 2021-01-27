using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// maybe rename to mission archive manager 
public class ArchivedMissionManager : MonoBehaviour, IDataModelManager
{
    List<ArchivedMission> ArchivedMissions { get; set; } // field vs. prop auto-imp

    void IDataModelManager.DeleteData()
    {
        throw new System.NotImplementedException();
    }

    void IDataModelManager.Init()
    {
        throw new System.NotImplementedException();
    }

    public async void LoadDataAsync()
    {
        foreach (Mission mission in MissionsManager.Instance.Missions
            .Where(m => m.NumberOfCompletions > 0))
        {
            ArchivedMission newArchivedMission = 
                new ArchivedMission($"{mission.Name}_{mission.NumberOfCompletions}");
            
            await newArchivedMission.LoadDataAsync();
            ArchivedMissions.Add(newArchivedMission);
        }
    }

    public void SaveData() => ArchivedMissions.ForEach(a => a.SaveData());
}
