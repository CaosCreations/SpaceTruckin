using UnityEngine;

public class IsolatedRepairsUI : SubMenu, IRepairsUI
{
    [SerializeField]
    private Transform parentTransform;

    public void Init(Ship shipToRepair, RepairsMinigameType minigameType)
    {
        var instance = RepairsMinigamesManager.Instance.InitMinigame(minigameType, parentTransform);
        instance.SetLayerRecursively(UIConstants.RepairsMinigameLayer);
    }
}