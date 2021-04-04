using UnityEngine;

public class DataModelManagerInitialiser : MonoBehaviour
{
    private void Start()
    {
        InitDataModelManagers();
    }

    private void InitDataModelManagers()
    {
        MissionsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();
        PilotTextManager.Instance.Init();
        PilotsManager.Instance.Init();
        ShipsManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
    }
}
