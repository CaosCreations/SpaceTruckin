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
        // Temporary randomisation for testing 
        //minigameType = PilotUtils.GetRandomEnumElement<RepairsMinigameType>();
        minigameType = Random.Range(0, 2) == 1 ? RepairsMinigameType.Stack : RepairsMinigameType.Tile;

        minigameInstance = IsolatedRepairsMinigamesManager.Instance.InitMinigame(minigameType, minigameInstanceParent);
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