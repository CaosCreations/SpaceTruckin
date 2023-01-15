using Cinemachine;
using UnityEngine;

public class IsolatedRepairsUI : SubMenu, IRepairsUI
{
    [SerializeField]
    private Transform minigameInstanceParent;

    private GameObject minigameInstance;

    [SerializeField]
    private Camera minigameInstanceCamera;

    public void Init(Ship shipToRepair, RepairsMinigameType minigameType)
    {
        minigameInstance = IsolatedRepairsMinigamesManager.Instance.InitMinigame(minigameType, minigameInstanceParent);
        //minigameInstance.SetLayerRecursively(UIConstants.RepairsMinigameLayer);
        //SetupCamera();
    }

    private void SetupCamera()
    {
        var virtualCamera = minigameInstance.GetComponentInChildren<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Priority = UIConstants.RepairsMinigameCameraOnPriority;
        }
        minigameInstanceCamera.transform.position = new Vector3(
            minigameInstance.transform.position.x, minigameInstance.transform.position.y, minigameInstance.transform.position.z - 5);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (minigameInstance != null)
        {
            Destroy(minigameInstance);
        }
    }
}