using UnityEngine;

public class DataModelManagerInitialiser : MonoBehaviour
{
    private void Start()
    {
        PilotNameManager.OnPilotNamesLoaded += PilotsManager.Instance.Init;
        InitDataModelManagers();
    }

    private void InitDataModelManagers()
    {
        MissionsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();

        PilotNameManager.Instance.Init();
        //PilotsManager.Instance.Init();

        ShipsManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
    }
}
