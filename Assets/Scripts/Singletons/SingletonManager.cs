using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    private void Start()
    {
        InitSingletons();

        CalendarManager.OnEndOfDay += SaveAllSingletonData;
    }

    private void InitSingletons()
    {
        MissionsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();
        PilotAssetsManager.Instance.Init();
        ShipsManager.Instance.Init();
        HangarManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
        CalendarManager.Instance.Init();
    }

    public static void SaveAllSingletonData()
    {
        PlayerManager.Instance.SaveData();
        MissionsManager.Instance.SaveData();
        PilotsManager.Instance.SaveData();
        ShipsManager.Instance.SaveData();
        HangarManager.Instance.SaveBatteryData();
        MessagesManager.Instance.SaveData();
        LicencesManager.Instance.SaveData();
        CalendarManager.Instance.SaveData();
    }
}
