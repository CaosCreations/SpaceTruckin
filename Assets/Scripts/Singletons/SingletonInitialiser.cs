using UnityEngine;

public class SingletonInitialiser : MonoBehaviour
{
    private void Start()
    {
        InitSingletons();
    }

    private void InitSingletons()
    {
        MissionsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();
        PilotAssetsManager.Instance.Init();
        PilotsManager.Instance.Init();
        ShipsManager.Instance.Init();
        HangarManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
    }
}
