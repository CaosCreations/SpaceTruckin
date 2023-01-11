using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    [SerializeField]
    private bool saveDataEnabled;

    private void Start()
    {
        if (saveDataEnabled)
        {
            LoadAllSingletonData();
            CalendarManager.OnEndOfDay += SaveAllSingletonData;
        }
        InitSingletons();
    }

    private void InitSingletons()
    {
        MissionsManager.Instance.Init();
        PilotsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();
        PilotAssetsManager.Instance.Init();
        ShipsManager.Instance.Init();
        HangarManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
        CalendarManager.Instance.Init();
    }

    private void LoadAllSingletonData()
    {
        MissionsManager.Instance.LoadDataAsync();
        PilotsManager.Instance.LoadDataAsync();
        ArchivedMissionsManager.Instance.LoadDataAsync();
        ShipsManager.Instance.LoadDataAsync();
        HangarManager.Instance.LoadBatteryDataAsync();
        MessagesManager.Instance.LoadDataAsync();
        PlayerManager.Instance.LoadDataAsync();
        CalendarManager.Instance.LoadDataAsync();
    }

    public static void SaveAllSingletonData()
    {
        PlayerManager.Instance.SaveData();
        MissionsManager.Instance.SaveData();
        ArchivedMissionsManager.Instance.SaveData();
        PilotsManager.Instance.SaveData();
        ShipsManager.Instance.SaveData();
        HangarManager.Instance.SaveBatteryData();
        MessagesManager.Instance.SaveData();
        LicencesManager.Instance.SaveData();
        CalendarManager.Instance.SaveData();
    }
}
