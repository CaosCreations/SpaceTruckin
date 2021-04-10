using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance { get; private set; }

    private void Awake()
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

    private void EndDay()
    {
        PlayerManager.Instance.SaveData();
        MissionsManager.Instance.SaveData();
        ArchivedMissionsManager.Instance.SaveData();
        PilotsManager.Instance.SaveData();
        ShipsManager.Instance.SaveData();
        MessagesManager.Instance.SaveData();
        LicencesManager.Instance.SaveData();
    }
}
