using Cinemachine;
using UnityEngine;

public class IsolatedRepairsUI : SubMenu, IRepairsUI
{
    [SerializeField]
    private Transform parentTransform;

    private GameObject minigameInstance;

    public void Init(Ship shipToRepair, RepairsMinigameType minigameType)
    {
        minigameInstance = IsolatedRepairsMinigamesManager.Instance.InitMinigame(minigameType, parentTransform);
        minigameInstance.SetLayerRecursively(UIConstants.RepairsMinigameLayer);
        SetCameraPriority();
    }

    private void SetCameraPriority()
    {
        var virtualCamera = minigameInstance.GetComponentInChildren<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Priority = UIConstants.RepairsMinigameCameraOnPriority;
        }
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