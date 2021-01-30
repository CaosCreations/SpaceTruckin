using UnityEngine;

// Rename: DataModelsLoader?
public class DataModelsManagerManager : MonoBehaviour
{
    private void Awake()
    {
        InitDataModelsManagers();
    }

    // What is the correct order?
    private void InitDataModelsManagers()
    {
        MissionsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();
        ShipsManager.Instance.Init();
        PilotsManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
    }
}
