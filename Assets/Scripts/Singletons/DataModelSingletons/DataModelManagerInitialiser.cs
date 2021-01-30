using UnityEngine;

public class DataModelManagerInitialiser : MonoBehaviour
{
    private void Start()
    {
        InitDataModelManagers();
    }

    private void InitDataModelManagers()
    {
        // MissionsManager must exist before ArchivedMissionsManager.
        MissionsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();

        ShipsManager.Instance.Init();
        PilotsManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
    }
}
